using Ether.Network.Packets;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.Core.Network.Packets.World;
using Rhisis.World.Systems.Inventory;
using Rhisis.World.Systems.Inventory.EventArgs;

namespace Rhisis.World.Handlers
{
    public static class InventoryHandler
    {
        [PacketHandler(PacketType.MOVEITEM)]
        public static void OnMoveItem(WorldClient client, INetPacketStream packet)
        {
            var moveItemPacket = new MoveItemPacket(packet);
            var inventoryEvent = new InventoryMoveEventArgs(moveItemPacket.SourceSlot, moveItemPacket.DestinationSlot);

            client.Player.NotifySystem<InventorySystem>(inventoryEvent);
        }

        [PacketHandler(PacketType.DOEQUIP)]
        public static void OnDoEquip(WorldClient client, INetPacketStream packet)
        {
            var equipItemPacket = new EquipItemPacket(packet);
            var inventoryEvent = new InventoryEquipEventArgs(equipItemPacket.UniqueId, equipItemPacket.Part);

            client.Player.NotifySystem<InventorySystem>(inventoryEvent);
        }
    }
}
