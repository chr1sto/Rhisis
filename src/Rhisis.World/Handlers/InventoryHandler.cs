using Ether.Network.Packets;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.Core.Network.Packets.World;
using Rhisis.World.Systems;
using Rhisis.World.Systems.Events;

namespace Rhisis.World.Handlers
{
    public static class InventoryHandler
    {
        [PacketHandler(PacketType.MOVEITEM)]
        public static void OnMoveItem(WorldClient client, NetPacketBase packet)
        {
            var moveItemPacket = new MoveItemPacket(packet);
            var inventoryEvent = new InventoryEventArgs(InventoryActionType.MoveItem, moveItemPacket.SourceSlot, moveItemPacket.DestinationSlot);

            client.Player.Context.NotifySystem<InventorySystem>(client.Player, inventoryEvent);
        }
    }
}
