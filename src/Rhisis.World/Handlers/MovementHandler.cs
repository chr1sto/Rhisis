using Ether.Network.Packets;
using Rhisis.Core.Exceptions;
using Rhisis.Core.Network.Packets.World;
using Rhisis.Core.Structures;
using Rhisis.World.Core.Components;
using Rhisis.World.Packets;

namespace Rhisis.World.Handlers
{
    public static class MovementHandler
    {
        public static void OnSnapshotSetDestPosition(WorldClient client, NetPacketBase packet)
        {
            var setDestPositionPacket = new SetDestPositionPacket(packet);
            var movableComponent = client.Player.GetComponent<MovableComponent>();

            if (movableComponent == null)
                throw new RhisisException($"The player doesn't have any {nameof(MovableComponent)} attached.");
            
            movableComponent.DestinationPosition = new Vector3(setDestPositionPacket.X, setDestPositionPacket.Y, setDestPositionPacket.Z);

            WorldPacketFactory.SendDestinationPosition(client, client.Player);
        }
    }
}
