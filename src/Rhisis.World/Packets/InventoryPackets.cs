using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;

namespace Rhisis.World.Handlers
{
    public static partial class WorldPacketFactory
    {
        public static void SendItemMove(IPlayerEntity entity, byte sourceSlot, byte destinationSlot)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(entity.Id, SnapshotType.MOVEITEM);
                packet.Write<byte>(0); // item type (not used)
                packet.Write(sourceSlot);
                packet.Write(destinationSlot);

                entity.Connection.Send(packet);
            }
        }

        public static void SendItemEquip(IPlayerEntity entity, Item item, int targetSlot, bool equip)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(entity.Id, SnapshotType.DOEQUIP);
                packet.Write(item.UniqueId);
                packet.Write<byte>(0); // Guild id
                packet.Write(equip ? (byte)0x01 : (byte)0x00);
                packet.Write(item.Id);
                packet.Write<short>(item.Refine); // Refine
                packet.Write(item.Element); // element
                packet.Write(item.ElementRefine); // element refine
                packet.Write(0); // flag
                packet.Write(targetSlot);

                entity.Connection.Send(packet);
                SendToVisible(packet, entity);
            }
        }

        public static void SendItemCreation(IPlayerEntity entity, Item item)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(entity.Id, SnapshotType.CREATEITEM);
                packet.Write<byte>(0);
                packet.Write(-1); // object id
                item.Serialize(packet);
                packet.Write<byte>(1);
                packet.Write((byte)item.UniqueId);
                packet.Write((short)item.Quantity);

                entity.Connection.Send(packet);
            }
        }
    }
}
