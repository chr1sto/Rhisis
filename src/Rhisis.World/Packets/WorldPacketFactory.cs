using Ether.Network.Common;
using Ether.Network.Packets;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;
using Rhisis.World.Game.Entities;
using System.Collections.Generic;
using System.Linq;
using Rhisis.Core.Data;

namespace Rhisis.World.Packets
{
    public static partial class WorldPacketFactory
    {
        public static void SendToVisible(INetPacketStream packet, IEntity player)
        {
            IEnumerable<IPlayerEntity> visiblePlayers = from x in player.ObjectComponent.Entities
                                                        where x.Type == WorldEntityType.Player
                                                        select x as IPlayerEntity;

            foreach (IPlayerEntity visiblePlayer in visiblePlayers)
                visiblePlayer.Connection.Send(packet);
        }

        public static void SendDestinationPosition(NetUser client, IMovableEntity movableEntity)
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

        public static void SendDefinedText(IPlayerEntity entity, int textId)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(entity.Id, SnapshotType.DEFINEDTEXT);
                packet.Write(textId);
                packet.Write(0);

                entity.Connection.Send(packet);
            }
        }

        public static void SendDefinedText(IPlayerEntity entity, DefineText text) => SendDefinedText(entity, (int)text);

        public static void SendUpdateAttributes(IPlayerEntity entity, DefineAttributes attribute, int newValue)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(entity.Id, SnapshotType.SETPOINTPARAM);
                packet.Write((int)attribute);
                packet.Write(newValue);

                entity.Connection.Send(packet);
                SendToVisible(packet, entity);
            }
        }
    }
}
