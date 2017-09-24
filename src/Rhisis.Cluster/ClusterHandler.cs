using Ether.Network.Packets;
using Rhisis.Cluster.Packets;
using Rhisis.Core.IO;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.Core.Network.Packets.Cluster;
using Rhisis.Database;
using Rhisis.Database.Structures;
using System;

namespace Rhisis.Cluster
{
    public static class ClusterHandler
    {
        [PacketHandler(PacketType.PING)]
        public static void OnPing(ClusterClient client, NetPacketBase packet)
        {
            var pingPacket = new PingPacket(packet);

            ClusterPacketFactory.SendPong(client, pingPacket.Time);
        }

        [PacketHandler(PacketType.GETPLAYERLIST)]
        public static void OnGetPlayerList(ClusterClient client, NetPacketBase packet)
        {
            var getPlayerListPacket = new GetPlayerListPacket(packet);

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
            }
        }

        [PacketHandler(PacketType.CREATE_PLAYER)]
        public static void OnCreatePlayer(ClusterClient client, NetPacketBase packet)
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
                    ClusterPacketFactory.SendError(client, ErrorType.USER_EXISTS);
                    client.Disconnect();
                    return;
                }

                character = new Character()
                {
                    UserId = userAccount.Id,
                    Name = createPlayerPacket.Name,
                    Hp = 100,
                    Mp = 100,
                    Fp = 100,
                    Strength = 15,
                    Stamina = 15,
                    Dexterity = 15,
                    Intelligence = 15,
                    MapId = 1,
                    PosX = 0f,
                    PosY = 0f,
                    PosZ = 0f,
                    SkinSetId = createPlayerPacket.SkinSet,
                    HairColor = createPlayerPacket.HairColor,
                    FaceId = createPlayerPacket.FaceId,
                    HairId = createPlayerPacket.HairMeshId,
                    Level = 1,
                    Gold = 0,
                    Experience = 0,
                    BankCode = createPlayerPacket.BankPassword,
                    Gender = createPlayerPacket.Gender,
                    ClassId = createPlayerPacket.Job,
                    StatPoints = 0,
                    SkillPoints = 0,
                    Slot = createPlayerPacket.Slot,
                };

                db.Characters.Create(character);

                Logger.Info($"User '{userAccount.Username} create character '{character.Name}'");
            }
        }

        [PacketHandler(PacketType.DEL_PLAYER)]
        public static void OnDeletePlayer(ClusterClient client, NetPacketBase packet)
        {
            var deletePlayerPacket = new DeletePlayerPacket(packet);


        }
    }
}
