using Ether.Network;
using Ether.Network.Packets;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
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

        public static void SendDestinationPosition(NetConnection client, IMovableEntity movableEntity)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(movableEntity.Id, SnapshotType.DESTPOS);
                packet.Write(movableEntity.MovableComponent.DestinationPosition.X);
                packet.Write(movableEntity.MovableComponent.DestinationPosition.Y);
                packet.Write(movableEntity.MovableComponent.DestinationPosition.Z);
                packet.Write<byte>(1);

                SendToVisible(packet, movableEntity);
            }
        }
    }
}
