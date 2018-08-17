using Ether.Network.Packets;
using NLog;
using Rhisis.Cluster.Packets;
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
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        [PacketHandler(PacketType.PING)]
        public static void OnPing(ClusterClient client, INetPacketStream packet)
        {
            var pak = new PingPacket(packet);

            if (!pak.IsTimeOut)
                CommonPacketFactory.SendPong(client, pak.Time);
        }

        [PacketHandler(PacketType.GETPLAYERLIST)]
        public static void OnGetPlayerList(ClusterClient client, INetPacketStream packet)
        {
            var pak = new GetPlayerListPacket(packet);
            WorldServerInfo selectedWorldServer = ClusterServer.GetWorldServerById(pak.ServerId);

            // Check if asked World server is still connected.
            if (selectedWorldServer == null)
            {
                Logger.Warn($"Unable to get characters list for user '{pak.Username}' from {client.RemoteEndPoint}. " +
                    "Reason: client requested the list on a not connected World server.");
                client.Disconnect();
                return;
            }

            using (var db = DatabaseService.GetContext())
            {
                User dbUser = db.Users.Get(x => x.Username.Equals(pak.Username, StringComparison.OrdinalIgnoreCase));

                // Check if user exist.
                if (dbUser == null)
                {
                    Logger.Warn($"[SECURITY] Unable to create new character for user '{pak.Username}' from {client.RemoteEndPoint}. " +
                        "Reason: bad presented credentials compared to the database.");
                    client.Disconnect();
                    return;
                }

                Logger.Debug($"Send character list to user '{pak.Username}' from {client.RemoteEndPoint}.");
                ClusterPacketFactory.SendPlayerList(client, pak.AuthenticationKey, dbUser.Characters);
                ClusterPacketFactory.SendWorldAddress(client, selectedWorldServer.Host);

                if (client.Configuration.EnableLoginProtect)
                    ClusterPacketFactory.SendLoginNumPad(client, client.LoginProtectValue);
            }
        }

        [PacketHandler(PacketType.CREATE_PLAYER)]
        public static void OnCreatePlayer(ClusterClient client, INetPacketStream packet)
        {
            var pak = new CreatePlayerPacket(packet);

            using (var db = DatabaseService.GetContext())
            {
                User dbUser = db.Users.Get(x => 
                    x.Username.Equals(pak.Username, StringComparison.OrdinalIgnoreCase) && 
                    x.Password.Equals(pak.Password, StringComparison.OrdinalIgnoreCase));

                // Check if user exist and with good password in database.
                if (dbUser == null)
                {
                    Logger.Warn($"[SECURITY] Unable to create new character for user '{pak.Username}' from {client.RemoteEndPoint}. " +
                        "Reason: bad presented credentials compared to the database.");
                    client.Disconnect();
                    return;
                }
                
                Character dbCharacter = db.Characters.Get(x => x.Name == pak.Name);

                // Check if character name is not already used.
                if (dbCharacter != null)
                {
                    Logger.Warn($"Unable to create new character for user '{pak.Username}' from {client.RemoteEndPoint}. " +
                        $"Reason: character name '{pak.Name}' already exists.");
                    ClusterPacketFactory.SendError(client, ErrorType.USER_EXISTS);
                    return;
                }

                DefaultCharacter defaultCharacter = client.Configuration.DefaultCharacter;
                DefaultStartItem defaultEquipment = pak.Gender == 0 ? defaultCharacter.Man : defaultCharacter.Woman;

                dbCharacter = new Character()
                {
                    UserId = dbUser.Id,
                    Name = pak.Name,
                    Slot = pak.Slot,
                    SkinSetId = pak.SkinSet,
                    HairColor = (int)pak.HairColor,
                    FaceId = pak.HeadMesh,
                    HairId = pak.HairMeshId,
                    BankCode = pak.BankPassword,
                    Gender = pak.Gender,
                    ClassId = pak.Job,
                    Hp = 100, //TODO: create game constants.
                    Mp = 100, //TODO: create game constants.
                    Fp = 100, //TODO: create game constants.
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
                    StatPoints = 0, //TODO: create game constants.
                    SkillPoints = 0, //TODO: create game constants.
                    Experience = 0, //TODO: create game constants.
                };

                //TODO: create game constants for slot.
                dbCharacter.Items.Add(new Item(defaultEquipment.StartSuit, 44));
                dbCharacter.Items.Add(new Item(defaultEquipment.StartHand, 46));
                dbCharacter.Items.Add(new Item(defaultEquipment.StartShoes, 47));
                dbCharacter.Items.Add(new Item(defaultEquipment.StartWeapon, 52));

                db.Characters.Create(dbCharacter);
                Logger.Info("Character '{0}' has been created successfully for user '{1}' from {2}.",
                    dbCharacter.Name, pak.Username, client.RemoteEndPoint );

                ClusterPacketFactory.SendPlayerList(client, pak.AuthenticationKey, dbUser.Characters);
            }
        }

        [PacketHandler(PacketType.DEL_PLAYER)]
        public static void OnDeletePlayer(ClusterClient client, INetPacketStream packet)
        {
            var pak = new DeletePlayerPacket(packet);

            using (var db = DatabaseService.GetContext())
            {
                User dbUser = db.Users.Get(x =>
                    x.Username.Equals(pak.Username, StringComparison.OrdinalIgnoreCase) &&
                    x.Password.Equals(pak.Password, StringComparison.OrdinalIgnoreCase));

                // Check if user exist and with good password in database.
                if (dbUser == null)
                {
                    Logger.Warn($"[SECURITY] Unable to delete character id '{pak.CharacterId}' for user '{pak.Username}' from {client.RemoteEndPoint}. " +
                        "Reason: bad presented credentials compared to the database.");
                    client.Disconnect();
                    return;
                }

                // Check if given password match confirmation password.
                if (!string.Equals(pak.Password, pak.PasswordConfirmation, StringComparison.OrdinalIgnoreCase))
                {
                    Logger.Warn($"Unable to delete character id '{pak.CharacterId}' for user '{pak.Username}' from {client.RemoteEndPoint}. " +
                        "Reason: passwords entered do not match.");
                    ClusterPacketFactory.SendError(client, ErrorType.WRONG_PASSWORD);
                    return;
                }
                
                Character dbCharacter = db.Characters.Get(pak.CharacterId);

                // Check if character exist.
                if (dbCharacter == null)
                {
                    Logger.Warn($"[SECURITY] Unable to delete character id '{pak.CharacterId}' for user '{pak.Username}' from {client.RemoteEndPoint}. " +
                        "Reason: user doesn't have any character with this id.");
                    client.Disconnect();
                    return;
                }

                db.Characters.Delete(dbCharacter);
                Logger.Info("Character '{0}' has been deleted successfully for user '{1}' from {2}.",
                    dbCharacter.Name, pak.Username, client.RemoteEndPoint);

                ClusterPacketFactory.SendPlayerList(client, pak.AuthenticationKey, dbUser.Characters);
            }
        }

        [PacketHandler(PacketType.PRE_JOIN)]
        public static void OnPreJoin(ClusterClient client, INetPacketStream packet)
        {
            using (var db = DatabaseService.GetContext())
            if (client.Configuration.EnableLoginProtect)
            {
                var pak = new PreJoinPacket(packet);
                Character dbCharacter = db.Characters.Get(pak.CharacterId);

                // Check if character exist.
                if (dbCharacter == null)
                {
                    Logger.Warn($"[SECURITY] Unable to prejoin character id '{pak.CharacterName}' for user '{pak.Username}' from {client.RemoteEndPoint}. " +
                        $"Reason: no character with id {pak.CharacterId}.");
                    client.Disconnect();
                    return;
                }

                // Check if given username is the real owner of this character.
                if (!pak.Username.Equals(dbCharacter.User.Username, StringComparison.OrdinalIgnoreCase))
                {
                    Logger.Warn($"[SECURITY] Unable to prejoin character '{dbCharacter.Name}' for user '{pak.Username}' from {client.RemoteEndPoint}. " +
                        "Reason: character is not owned by this user.");
                    client.Disconnect();
                    return;
                }

                // Check if presented bank code is correct.
                if (LoginProtect.GetNumPadToPassword(client.LoginProtectValue, pak.BankCode) != dbCharacter.BankCode)
                {
                    Logger.Warn($"Unable to prejoin character '{dbCharacter.Name}' for user '{pak.Username}' from {client.RemoteEndPoint}. " +
                        "Reason: bad bank code.");
                    client.LoginProtectValue = new Random().Next(0, 1000);
                    ClusterPacketFactory.SendLoginProtect(client, client.LoginProtectValue);
                    return;
                }

                ClusterPacketFactory.SendJoinWorld(client);
                Logger.Info("Character '{0}' has prejoin successfully the game for user '{1}' from {2}.",
                    dbCharacter.Name, pak.Username, client.RemoteEndPoint);
            }
            else
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
                    
                    ClusterPacketFactory.SendJoinWorld(client);
                }
            }
        }
    }
}
