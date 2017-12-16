using Ether.Network.Packets;
using Rhisis.Core.Network.Packets.World;

namespace Rhisis.World.Handlers
{
    public static class MovementHandler
    {
        public static void OnSnapshotSetDestPosition(WorldClient client, NetPacketBase packet)
        {
            var setDestPositionPacket = new SetDestPositionPacket(packet);
            //var objectComponent = client.Player.GetComponent<ObjectComponent>();
            //var movableComponent = client.Player.GetComponent<MovableComponent>();

            //if (objectComponent == null)
            //    throw new RhisisException($"The player doesn't have any {nameof(ObjectComponent)} attached.");

            //if (movableComponent == null)
            //    throw new RhisisException($"The player doesn't have any {nameof(MovableComponent)} attached.");
            
            //movableComponent.DestinationPosition = new Vector3(setDestPositionPacket.X, setDestPositionPacket.Y, setDestPositionPacket.Z);
            //objectComponent.Angle = Vector3.AngleBetween(objectComponent.Position, movableComponent.DestinationPosition);

            //WorldPacketFactory.SendDestinationPosition(client, client.Player);
        }
    }
}
