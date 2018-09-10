using Ether.Network.Packets;
using NLog;
using Rhisis.Cluster.Packets;
using Rhisis.Network.ISC.Structures;
using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.Network.Packets.Cluster;
using Rhisis.Core.Structures;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.Database.Repositories;
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

            var userRepository = new UserRepository();
            DbUser dbUser = userRepository.Get(x => x.Username.Equals(pak.Username, StringComparison.OrdinalIgnoreCase));

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

        [PacketHandler(PacketType.CREATE_PLAYER)]
        public static void OnCreatePlayer(ClusterClient client, INetPacketStream packet)
        {
            var pak = new CreatePlayerPacket(packet);

            using (var db = DatabaseFactory.Instance.CreateDbContext())
            {
                var userRepository = new UserRepository(db);
                var characterRepository = new CharacterRepository(db);

                DbUser dbUser = userRepository.Get(x =>
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

                DbCharacter dbCharacter = characterRepository.Get(x => x.Name == pak.Name);

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

                dbCharacter = new DbCharacter()
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
                dbCharacter.Items.Add(new DbItem(defaultEquipment.StartSuit, 44));
                dbCharacter.Items.Add(new DbItem(defaultEquipment.StartHand, 46));
                dbCharacter.Items.Add(new DbItem(defaultEquipment.StartShoes, 47));
                dbCharacter.Items.Add(new DbItem(defaultEquipment.StartWeapon, 52));

                characterRepository.Create(dbCharacter);
                Logger.Info("Character '{0}' has been created successfully for user '{1}' from {2}.",
                    dbCharacter.Name, pak.Username, client.RemoteEndPoint);

                ClusterPacketFactory.SendPlayerList(client, pak.AuthenticationKey, dbUser.Characters);
            }
        }

        [PacketHandler(PacketType.DEL_PLAYER)]
        public static void OnDeletePlayer(ClusterClient client, INetPacketStream packet)
        {
            var pak = new DeletePlayerPacket(packet);

            using (var db = DatabaseFactory.Instance.CreateDbContext())
            {
                var userRepository = new UserRepository(db);
                var characterRepository = new CharacterRepository(db);
                DbUser dbUser = userRepository.Get(x =>
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

                DbCharacter dbCharacter = characterRepository.Get(pak.CharacterId);

                // Check if character exist.
                if (dbCharacter == null)
                {
                    Logger.Warn($"[SECURITY] Unable to delete character id '{pak.CharacterId}' for user '{pak.Username}' from {client.RemoteEndPoint}. " +
                        "Reason: user doesn't have any character with this id.");
                    client.Disconnect();
                    return;
                }

                characterRepository.Delete(dbCharacter);
                Logger.Info("Character '{0}' has been deleted successfully for user '{1}' from {2}.",
                    dbCharacter.Name, pak.Username, client.RemoteEndPoint);

                ClusterPacketFactory.SendPlayerList(client, pak.AuthenticationKey, dbUser.Characters);
            }
        }

        [PacketHandler(PacketType.PRE_JOIN)]
        public static void OnPreJoin(ClusterClient client, INetPacketStream packet)
        {
            var pak = new PreJoinPacket(packet);
            var characterRepository = new CharacterRepository();

            DbCharacter dbCharacter = characterRepository.Get(pak.CharacterId);

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
            if (client.Configuration.EnableLoginProtect &&
                LoginProtect.GetNumPadToPassword(client.LoginProtectValue, pak.BankCode) != dbCharacter.BankCode)
            {
                Logger.Warn($"Unable to prejoin character '{dbCharacter.Name}' for user '{pak.Username}' from {client.RemoteEndPoint}. " +
                    "Reason: bad bank code.");
                client.LoginProtectValue = new Random().Next(0, 1000);
                ClusterPacketFactory.SendLoginProtect(client, client.LoginProtectValue);
                return;
            }

            // Finally, we connect the player.
            ClusterPacketFactory.SendJoinWorld(client);
            Logger.Info("Character '{0}' has prejoin successfully the game for user '{1}' from {2}.",
                dbCharacter.Name, pak.Username, client.RemoteEndPoint);
        }
    }
}
