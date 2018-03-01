using Ether.Network.Packets;
using Rhisis.Core.Network.Packets.World;
using Rhisis.Core.Structures;
using Rhisis.World.Handlers;

namespace Rhisis.World.Handlers
{
    public static class MovementHandler
    {
        public static void OnSnapshotSetDestPosition(WorldClient client, NetPacketBase packet)
        {
            var setDestPositionPacket = new SetDestPositionPacket(packet);

            client.Player.MovableComponent.DestinationPosition = new Vector3(setDestPositionPacket.X, setDestPositionPacket.Y, setDestPositionPacket.Z);
            client.Player.ObjectComponent.Angle = Vector3.AngleBetween(client.Player.ObjectComponent.Position, client.Player.MovableComponent.DestinationPosition);

            WorldPacketFactory.SendDestinationPosition(client, client.Player);
        }
    }
}
