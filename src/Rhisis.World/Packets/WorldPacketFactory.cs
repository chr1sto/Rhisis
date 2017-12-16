using Ether.Network;
using Ether.Network.Packets;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;
using Rhisis.World.Game.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Packets
{
    public static partial class WorldPacketFactory
    {
        public static void SendToVisible(NetPacketBase packet, IEntity player)
        {
            IEnumerable<IPlayerEntity> visiblePlayers = from x in player.ObjectComponent.Entities
                                                        where x.Type == WorldEntityType.Player
                                                        select x as IPlayerEntity;

            foreach (var visiblePlayer in visiblePlayers)
                visiblePlayer.PlayerComponent.Connection.Send(packet);
        }

        public static void SendDestinationPosition(NetConnection client, IEntity player)
        {
            //var objectComponent = player.GetComponent<ObjectComponent>();
            //var movableComponent = player.GetComponent<MovableComponent>();

            //using (var packet = new FFPacket())
            //{
            //    packet.StartNewMergedPacket(player.Id, SnapshotType.DESTPOS);
            //    packet.Write(movableComponent.DestinationPosition.X);
            //    packet.Write(movableComponent.DestinationPosition.Y);
            //    packet.Write(movableComponent.DestinationPosition.Z);
            //    packet.Write<byte>(1);

            //    SendToVisible(packet, player);
            //}
        }
    }
}
