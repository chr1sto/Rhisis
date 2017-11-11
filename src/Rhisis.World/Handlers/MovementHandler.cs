using Ether.Network.Packets;
using Rhisis.Core.Exceptions;
using Rhisis.Core.Structures;
using Rhisis.World.Core.Components;
using Rhisis.World.Packets;
using System;

namespace Rhisis.World.Handlers
{
    public static class MovementHandler
    {
        public static void OnSnapshotSetDestPosition(WorldClient client, NetPacketBase packet)
        {
            var movableComponent = client.Player.GetComponent<MovableComponent>();
            var posX = packet.Read<float>();
            var posY = packet.Read<float>();
            var posZ = packet.Read<float>();
            var forward = packet.Read<byte>();

            if (movableComponent == null)
                throw new RhisisException($"The player doesn't have any {nameof(MovableComponent)} attached.");

            movableComponent.DestinationPosition = new Vector3(posX, posY, posZ);

            WorldPacketFactory.SendDestinationPosition(client, client.Player);
        }
    }
}
