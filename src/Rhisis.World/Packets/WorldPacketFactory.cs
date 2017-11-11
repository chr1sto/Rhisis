using Ether.Network;
using Ether.Network.Packets;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.World.Core.Components;
using Rhisis.World.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rhisis.World.Packets
{
    public static partial class WorldPacketFactory
    {
        public static void SendToVisible(NetPacketBase packet, IEntity player)
        {
            var objectComponent = player.GetComponent<ObjectComponent>();
            IEnumerable<PlayerComponent> visiblePlayers = from x in objectComponent.Entities
                                                          where x.HasComponent<PlayerComponent>()
                                                          select x.GetComponent<PlayerComponent>();

            foreach (var visiblePlayer in visiblePlayers)
                visiblePlayer.Connection.Send(packet);
        }

        public static void SendDestinationPosition(NetConnection client, IEntity player)
        {
            var objectComponent = player.GetComponent<ObjectComponent>();
            var movableComponent = player.GetComponent<MovableComponent>();

            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(objectComponent.ObjectId, SnapshotType.DESTPOS);
                packet.Write(movableComponent.DestinationPosition.X);
                packet.Write(movableComponent.DestinationPosition.Y);
                packet.Write(movableComponent.DestinationPosition.Z);
                packet.Write<byte>(1);

                SendToVisible(packet, player);
            }
        }
    }
}
