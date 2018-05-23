using Ether.Network.Packets;
using Rhisis.Cluster.Packets;
using Rhisis.Core.IO;
using Rhisis.Core.ISC.Structures;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.Core.Network.Packets.Cluster;
using Rhisis.Core.Structures;
using Rhisis.Database;
using Rhisis.Database.Entities;
using System;

namespace Rhisis.Cluster
{
    public static class ClusterHandler
    {
        [PacketHandler(PacketType.PING)]
        public static void OnPing(ClusterClient client, INetPacketStream packet)
        {
            var pingPacket = new PingPacket(packet);

            CommonPacketFactory.SendPong(client, pingPacket.Time);
        }

        [PacketHandler(PacketType.GETPLAYERLIST)]
        public static void OnGetPlayerList(ClusterClient client, INetPacketStream packet)
        {
            var getPlayerListPacket = new GetPlayerListPacket(packet);
            WorldServerInfo selectedServer = ClusterServer.GetWorldServerById(getPlayerListPacket.ServerId);

            // TODO: verification

            using (var db = DatabaseService.GetContext())
            {
                User dbUser = db.Users.Get(x => x.Username.Equals(getPlayerListPacket.Username, StringComparison.OrdinalIgnoreCase));

                if (dbUser == null)
                {
                    Logger.Warning($"User '{getPlayerListPacket.Username}' logged with invalid credentials.");
                    client.Disconnect();
                    return;
                }

                ClusterPacketFactory.SendPlayerList(client, getPlayerListPacket.AuthenticationKey, dbUser.Characters);

                if (selectedServer != null)
                    ClusterPacketFactory.SendWorldAddress(client, selectedServer.Host);

                if (client.Configuration.EnableLoginProtect)
                    ClusterPacketFactory.SendLoginNumPad(client, client.LoginProtectValue);
            }
        }

        [PacketHandler(PacketType.CREATE_PLAYER)]
        public static void OnCreatePlayer(ClusterClient client, INetPacketStream packet)
        {
            var createPlayerPacket = new CreatePlayerPacket(packet);

            using (var db = DatabaseService.GetContext())
            {
                User userAccount = db.Users.Get(x => x.Username.Equals(createPlayerPacket.Username, StringComparison.OrdinalIgnoreCase) && x.Password.Equals(createPlayerPacket.Password, StringComparison.OrdinalIgnoreCase));

                if (userAccount == null)
                {
                    Logger.Warning($"User '{createPlayerPacket.Username}' logged with invalid credentials.");
                    client.Disconnect();
                    return;
                }

                // Check character name
                Character character = db.Characters.Get(x => x.Name == createPlayerPacket.Name);

                if (character != null)
                {
                    Logger.Info($"Character name '{createPlayerPacket.Name}' already exists.");
                    ClusterPacketFactory.SendError(client, ErrorType.INVALID_NAME_CHARACTER);
                    return;
                }

                DefaultCharacter defaultCharacter = client.Configuration.DefaultCharacter;
                DefaultStartItem defaultEquipment = createPlayerPacket.Gender == 0 ? defaultCharacter.Man : defaultCharacter.Woman;

                character = new Character()
                {
                    UserId = userAccount.Id,
                    Name = createPlayerPacket.Name,
                    Slot = createPlayerPacket.Slot,
                    SkinSetId = createPlayerPacket.SkinSet,
                    HairColor = (int)createPlayerPacket.HairColor,
                    FaceId = createPlayerPacket.HeadMesh,
                    HairId = createPlayerPacket.HairMeshId,
                    BankCode = createPlayerPacket.BankPassword,
                    Gender = createPlayerPacket.Gender,
                    ClassId = createPlayerPacket.Job,
                    Hp = 100,
                    Mp = 100,
                    Fp = 100,
                    Strength = defaultCharacter.Strength,
                    Stamina = defaultCharacter.Stamina,
                    Dexterity = defaultCharacter.Dexterity,
                    Intelligence = defaultCharacter.Intelligence,
                    MapId = defaultCharacter.MapId,
                    PosX = defaultCharacter.PosX,
                    PosY = defaultCharacter.PosY,
                    PosZ = defaultCharacter.PosZ,
                    Level = defaultCharacter.Level,
                    Gold = defaultCharacter.Gold,
                    StatPoints = 0,
                    SkillPoints = 0,
                    Experience = 0,
                };

                character.Items.Add(new Item(defaultEquipment.StartSuit, 44));
                character.Items.Add(new Item(defaultEquipment.StartHand, 46));
                character.Items.Add(new Item(defaultEquipment.StartShoes, 47));
                character.Items.Add(new Item(defaultEquipment.StartWeapon, 52));

                db.Characters.Create(character);

                Logger.Info($"User '{userAccount.Username} create character '{character.Name}'");
                ClusterPacketFactory.SendPlayerList(client, createPlayerPacket.AuthenticationKey, userAccount.Characters);
            }
        }

        [PacketHandler(PacketType.DEL_PLAYER)]
        public static void OnDeletePlayer(ClusterClient client, INetPacketStream packet)
        {
            var deletePlayerPacket = new DeletePlayerPacket(packet);

            using (var db = DatabaseService.GetContext())
            {
                User userAccount = db.Users.Get(x => x.Username.Equals(deletePlayerPacket.Username, StringComparison.OrdinalIgnoreCase) && x.Password.Equals(deletePlayerPacket.Password, StringComparison.OrdinalIgnoreCase));

                if (userAccount == null)
                {
                    Logger.Warning($"User '{deletePlayerPacket.Username}' logged with invalid credentials.");
                    client.Disconnect();
                    return;
                }

                if (!string.Equals(deletePlayerPacket.Password, deletePlayerPacket.PasswordConfirmation, StringComparison.OrdinalIgnoreCase))
                {
                    Logger.Info($"Invalid password confirmation for user '{userAccount.Username}");
                    ClusterPacketFactory.SendError(client, ErrorType.INVALID_NAME_CHARACTER);
                    return;
                }
                
                Character character = db.Characters.Get(deletePlayerPacket.CharacterId);

                if (character == null)
                {
                    Logger.Warning($"User '{userAccount.Username}' doesn't have any character with id '{deletePlayerPacket.CharacterId}'");
                    ClusterPacketFactory.SendError(client, ErrorType.INVALID_NAME_CHARACTER);
                    return;
                }

                db.Characters.Delete(character);
                ClusterPacketFactory.SendPlayerList(client, deletePlayerPacket.AuthenticationKey, userAccount.Characters);
            }
        }

        [PacketHandler(PacketType.PRE_JOIN)]
        public static void OnPreJoin(ClusterClient client, INetPacketStream packet)
        {
            if (client.Configuration.EnableLoginProtect)
            {
                var preJoinPacket = new PreJoinPacket(packet);

                using (var db = DatabaseService.GetContext())
                {
                    Character selectedCharacter = db.Characters.Get(preJoinPacket.CharacterId);

                    if (selectedCharacter == null)
                    {
                        Logger.Error("Cannot find character '{0}' with id {1} in database.", preJoinPacket.CharacterName, preJoinPacket.CharacterId);
                        return;
                    }

                    if (!selectedCharacter.User.Username.Equals(preJoinPacket.Username, StringComparison.OrdinalIgnoreCase))
                        return;

                    int realBankCode = LoginProtect.GetNumPadToPassword(client.LoginProtectValue, preJoinPacket.BankCode);
                    if (selectedCharacter.BankCode != realBankCode)
                    {
                        Logger.Error("Character '{0}' tried to connect with incorrect bank password.", selectedCharacter.Name);
                        client.LoginProtectValue = new Random().Next(0, 1000);
                        ClusterPacketFactory.SendLoginProtect(client, client.LoginProtectValue);
                        return;
                    }

                    ClusterPacketFactory.SendJoinWorld(client);
                }
            }
            else
                Logger.Warning("Simple authentication not implemented.");
        }
    }
}
