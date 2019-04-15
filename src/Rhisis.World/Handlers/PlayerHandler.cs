using Ether.Network.Packets;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Structures;
using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.Network.Packets.World;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Follow;
using Rhisis.World.Systems.PlayerData;
using Rhisis.World.Systems.PlayerData.EventArgs;

namespace Rhisis.World.Handlers
{
    internal class PlayerHandler
    {
        private static readonly ILogger Logger = DependencyContainer.Instance.Resolve<ILogger<PlayerHandler>>();

        [PacketHandler(PacketType.PLAYERSETDESTOBJ)]
        public static void OnPlayerSetDestObject(WorldClient client, INetPacketStream packet)
        {
            var targetObjectId = packet.Read<uint>();
            var distance = packet.Read<float>();
            var followEvent = new FollowEventArgs(targetObjectId, distance);

            client.Player.NotifySystem<FollowSystem>(followEvent);
        }

        [PacketHandler(PacketType.QUERY_PLAYER_DATA)]
        public static void OnQueryPlayerData(WorldClient client, INetPacketStream packet)
        {
            var onQueryPlayerDataPacket = new QueryPlayerDataPacket(packet);
            var queryPlayerDataEvent = new QueryPlayerDataEventArgs(onQueryPlayerDataPacket.PlayerId, onQueryPlayerDataPacket.Version);
            client.Player.NotifySystem<PlayerDataSystem>(queryPlayerDataEvent);
        }

        [PacketHandler(PacketType.QUERY_PLAYER_DATA2)]
        public static void OnQueryPlayerData2(WorldClient client, INetPacketStream packet)
        {
            var onQueryPlayerData2Packet = new QueryPlayerData2Packet(packet);
            var queryPlayerData2Event = new QueryPlayerData2EventArgs(onQueryPlayerData2Packet.Size, onQueryPlayerData2Packet.PlayerDictionary);
            client.Player.NotifySystem<PlayerDataSystem>(queryPlayerData2Event);
        }

        [PacketHandler(PacketType.PLAYERMOVED)]
        public static void OnPlayerMoved(WorldClient client, INetPacketStream packet)
        {
            var playerMovedPacket = new PlayerMovedPacket(packet);

            if (client.Player.Health.IsDead)
            {
                Logger.LogError($"Player {client.Player.Object.Name} is dead, he cannot move with keyboard.");
                return;
            }

            // TODO: Check if player is flying

            client.Player.Follow.Reset();
            client.Player.Battle.Reset();
            client.Player.Object.Position = playerMovedPacket.BeginPosition + playerMovedPacket.DestinationPosition;
            client.Player.Object.Angle = playerMovedPacket.Angle;
            client.Player.Object.MovingFlags = (ObjectState)playerMovedPacket.State;
            client.Player.Object.MotionFlags = (StateFlags)playerMovedPacket.StateFlag;
            client.Player.MovableComponent.IsMovingWithKeyboard = client.Player.Object.MovingFlags.HasFlag(ObjectState.OBJSTA_FMOVE) || 
                client.Player.Object.MovingFlags.HasFlag(ObjectState.OBJSTA_BMOVE);
            client.Player.MovableComponent.DestinationPosition = playerMovedPacket.BeginPosition + playerMovedPacket.DestinationPosition;

            WorldPacketFactory.SendMoverMoved(client.Player,
                playerMovedPacket.BeginPosition, 
                playerMovedPacket.DestinationPosition,
                client.Player.Object.Angle, 
                (uint)client.Player.Object.MovingFlags, 
                (uint)client.Player.Object.MotionFlags, 
                playerMovedPacket.Motion, 
                playerMovedPacket.MotionEx, 
                playerMovedPacket.Loop, 
                playerMovedPacket.MotionOption, 
                playerMovedPacket.TickCount);
        }
    }
}
