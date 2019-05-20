using Ether.Network.Packets;
using Rhisis.Network.Packets.World;
using Rhisis.Core.Structures;
using Rhisis.World.Packets;

namespace Rhisis.World.Handlers
{
    public static class MovementHandler
    {
        public static void OnSnapshotSetDestPosition(WorldClient client, INetPacketStream packet)
        {
            var setDestPositionPacket = new SetDestPositionPacket(packet);

            client.Player.Object.MovingFlags = Core.Data.ObjectState.OBJSTA_FMOVE;
            client.Player.Moves.DestinationPosition = new Vector3(setDestPositionPacket.X, setDestPositionPacket.Y, setDestPositionPacket.Z);
            client.Player.Object.Angle = Vector3.AngleBetween(client.Player.Object.Position, client.Player.Moves.DestinationPosition);
            client.Player.Follow.Target = null;

            WorldPacketFactory.SendDestinationPosition(client.Player);
        }
    }
}
