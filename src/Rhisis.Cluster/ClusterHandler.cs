using Ether.Network.Packets;
using Microsoft.Extensions.Logging;
using Rhisis.Cluster.Packets;
using Rhisis.Core.Common.Formulas;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Resources.Loaders;
using Rhisis.Core.Structures;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Core.Structures.Game;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.Network;
using Rhisis.Network.ISC.Structures;
using Rhisis.Network.Packets;
using Rhisis.Network.Packets.Cluster;
using System;

namespace Rhisis.Cluster
{
    public class ClusterHandler
    {
        private static readonly ILogger<ClusterHandler> Logger = DependencyContainer.Instance.Resolve<ILogger<ClusterHandler>>();

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
            var clusterServer = DependencyContainer.Instance.Resolve<IClusterServer>();
            var clusterConfiguration = DependencyContainer.Instance.Resolve<ClusterConfiguration>();
            WorldServerInfo selectedWorldServer = clusterServer.GetWorldServerById(pak.ServerId);

            // Check if asked World server is still connected.
            if (selectedWorldServer == null)
            {
                Logger.LogWarning($"Unable to get characters list for user '{pak.Username}' from {client.RemoteEndPoint}. " +
                    "Reason: client requested the list on a not connected World server.");
                client.Disconnect();
                return;
            }

            using (var database = DependencyContainer.Instance.Resolve<IDatabase>())
            {
                DbUser dbUser = database.Users.Get(x => x.Username.Equals(pak.Username, StringComparison.OrdinalIgnoreCase));

                // Check if user exist.
                if (dbUser == null)
                {
                    Logger.LogWarning($"[SECURITY] Unable to create new character for user '{pak.Username}' from {client.RemoteEndPoint}. " +
                        "Reason: bad presented credentials compared to the database.");
                    client.Disconnect();
                    return;
                }

                Logger.LogDebug($"Send character list to user '{pak.Username}' from {client.RemoteEndPoint}.");
                ClusterPacketFactory.SendPlayerList(client, pak.AuthenticationKey, dbUser.Characters);
                ClusterPacketFactory.SendWorldAddress(client, selectedWorldServer.Host);

                if (clusterConfiguration.EnableLoginProtect)
                    ClusterPacketFactory.SendLoginNumPad(client, client.LoginProtectValue);
            }
        }

        [PacketHandler(PacketType.CREATE_PLAYER)]
        public static void OnCreatePlayer(ClusterClient client, INetPacketStream packet)
        {
            var pak = new CreatePlayerPacket(packet);
            var clusterConfiguration = DependencyContainer.Instance.Resolve<ClusterConfiguration>();
            var jobs = DependencyContainer.Instance.Resolve<JobLoader>();

            using (var database = DependencyContainer.Instance.Resolve<IDatabase>())
            {
                DbUser dbUser = database.Users.Get(x =>
                    x.Username.Equals(pak.Username, StringComparison.OrdinalIgnoreCase) &&
                    x.Password.Equals(pak.Password, StringComparison.OrdinalIgnoreCase));

                // Check if user exist and with good password in database.
                if (dbUser == null)
                {
                    Logger.LogWarning($"[SECURITY] Unable to create new character for user '{pak.Username}' from {client.RemoteEndPoint}. " +
                        "Reason: bad presented credentials compared to the database.");
                    client.Disconnect();
                    return;
                }

                DbCharacter dbCharacter = database.Characters.Get(x => x.Name == pak.Name);

                // Check if character name is not already used.
                if (dbCharacter != null)
                {
                    Logger.LogWarning($"Unable to create new character for user '{pak.Username}' from {client.RemoteEndPoint}. " +
                        $"Reason: character name '{pak.Name}' already exists.");
                    ClusterPacketFactory.SendError(client, ErrorType.USER_EXISTS);
                    return;
                }

                DefaultCharacter defaultCharacter = clusterConfiguration.DefaultCharacter;
                DefaultStartItem defaultEquipment = pak.Gender == 0 ? defaultCharacter.Man : defaultCharacter.Woman;
                JobData jobData = jobs[pak.Job];

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
                    Hp = HealthFormulas.GetMaxOriginHp(defaultCharacter.Level, defaultCharacter.Stamina, jobData.MaxHpFactor),
                    Mp = HealthFormulas.GetMaxOriginMp(defaultCharacter.Level, defaultCharacter.Intelligence, jobData.MaxMpFactor, true),
                    Fp = HealthFormulas.GetMaxOriginFp(defaultCharacter.Level, defaultCharacter.Stamina, defaultCharacter.Dexterity, defaultCharacter.Strength, jobData.MaxFpFactor, true),
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
                    Experience = 0,
                };

                //TODO: create game constants for slot.
                dbCharacter.Items.Add(new DbItem(defaultEquipment.StartSuit, 44));
                dbCharacter.Items.Add(new DbItem(defaultEquipment.StartHand, 46));
                dbCharacter.Items.Add(new DbItem(defaultEquipment.StartShoes, 47));
                dbCharacter.Items.Add(new DbItem(defaultEquipment.StartWeapon, 52));

                database.Characters.Create(dbCharacter);
                database.Complete();
                Logger.LogInformation("Character '{0}' has been created successfully for user '{1}' from {2}.",
                    dbCharacter.Name, pak.Username, client.RemoteEndPoint);

                ClusterPacketFactory.SendPlayerList(client, pak.AuthenticationKey, dbUser.Characters);
            }
        }

        [PacketHandler(PacketType.DEL_PLAYER)]
        public static void OnDeletePlayer(ClusterClient client, INetPacketStream packet)
        {
            var pak = new DeletePlayerPacket(packet);

            using (var database = DependencyContainer.Instance.Resolve<IDatabase>())
            {
                DbUser dbUser = database.Users.Get(x =>
                    x.Username.Equals(pak.Username, StringComparison.OrdinalIgnoreCase) &&
                    x.Password.Equals(pak.Password, StringComparison.OrdinalIgnoreCase));

                // Check if user exist and with good password in database.
                if (dbUser == null)
                {
                    Logger.LogWarning($"[SECURITY] Unable to delete character id '{pak.CharacterId}' for user '{pak.Username}' from {client.RemoteEndPoint}. " +
                        "Reason: bad presented credentials compared to the database.");
                    client.Disconnect();
                    return;
                }

                // Check if given password match confirmation password.
                if (!string.Equals(pak.Password, pak.PasswordConfirmation, StringComparison.OrdinalIgnoreCase))
                {
                    Logger.LogWarning($"Unable to delete character id '{pak.CharacterId}' for user '{pak.Username}' from {client.RemoteEndPoint}. " +
                        "Reason: passwords entered do not match.");
                    ClusterPacketFactory.SendError(client, ErrorType.WRONG_PASSWORD);
                    return;
                }

                DbCharacter dbCharacter = database.Characters.Get(pak.CharacterId);

                // Check if character exist.
                if (dbCharacter == null)
                {
                    Logger.LogWarning($"[SECURITY] Unable to delete character id '{pak.CharacterId}' for user '{pak.Username}' from {client.RemoteEndPoint}. " +
                        "Reason: user doesn't have any character with this id.");
                    client.Disconnect();
                    return;
                }

                database.Characters.Delete(dbCharacter);
                database.Complete();
                Logger.LogInformation("Character '{0}' has been deleted successfully for user '{1}' from {2}.",
                    dbCharacter.Name, pak.Username, client.RemoteEndPoint);

                ClusterPacketFactory.SendPlayerList(client, pak.AuthenticationKey, dbUser.Characters);
            }
        }

        [PacketHandler(PacketType.PRE_JOIN)]
        public static void OnPreJoin(ClusterClient client, INetPacketStream packet)
        {
            var pak = new PreJoinPacket(packet);
            var clusterConfiguration = DependencyContainer.Instance.Resolve<ClusterConfiguration>();
            DbCharacter dbCharacter = null;

            using (var database = DependencyContainer.Instance.Resolve<IDatabase>())
                dbCharacter = database.Characters.Get(pak.CharacterId);

            // Check if character exist.
            if (dbCharacter == null)
            {
                Logger.LogWarning($"[SECURITY] Unable to prejoin character id '{pak.CharacterName}' for user '{pak.Username}' from {client.RemoteEndPoint}. " +
                    $"Reason: no character with id {pak.CharacterId}.");
                client.Disconnect();
                return;
            }

            // Check if given username is the real owner of this character.
            if (!pak.Username.Equals(dbCharacter.User.Username, StringComparison.OrdinalIgnoreCase))
            {
                Logger.LogWarning($"[SECURITY] Unable to prejoin character '{dbCharacter.Name}' for user '{pak.Username}' from {client.RemoteEndPoint}. " +
                    "Reason: character is not owned by this user.");
                client.Disconnect();
                return;
            }

            // Check if presented bank code is correct.
            if (clusterConfiguration.EnableLoginProtect &&
                LoginProtect.GetNumPadToPassword(client.LoginProtectValue, pak.BankCode) != dbCharacter.BankCode)
            {
                Logger.LogWarning($"Unable to prejoin character '{dbCharacter.Name}' for user '{pak.Username}' from {client.RemoteEndPoint}. " +
                    "Reason: bad bank code.");
                client.LoginProtectValue = new Random().Next(0, 1000);
                ClusterPacketFactory.SendLoginProtect(client, client.LoginProtectValue);
                return;
            }

            // Finally, we connect the player.
            ClusterPacketFactory.SendJoinWorld(client);
            Logger.LogInformation("Character '{0}' has prejoin successfully the game for user '{1}' from {2}.",
                dbCharacter.Name, pak.Username, client.RemoteEndPoint);
        }
    }
}
