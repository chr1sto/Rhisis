using Ether.Network.Packets;
using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.Network.Packets.World;
using Rhisis.World.Systems.NpcShop;
using Rhisis.World.Systems.NpcShop.EventArgs;

namespace Rhisis.World.Handlers
{
    public static class NpcShopHandler
    {
        [PacketHandler(PacketType.OPENSHOPWND)]
        public static void OnOpenShopWindow(WorldClient client, INetPacketStream packet)
        {
            var openShopPacket = new OpenShopWindowPacket(packet);
            var npcEvent = new NpcShopOpenEventArgs(openShopPacket.ObjectId);

            client.Player.NotifySystem<NpcShopSystem>(npcEvent);
        }

        [PacketHandler(PacketType.CLOSESHOPWND)]
        public static void OnCloseShopWindow(WorldClient client, INetPacketStream packet)
        {
            client.Player.NotifySystem<NpcShopSystem>(new NpcShopCloseEventArgs());
        }

        [PacketHandler(PacketType.BUYITEM)]
        public static void OnBuyItem(WorldClient client, INetPacketStream packet)
        {
            var buyItemPacket = new BuyItemPacket(packet);
            var npcShopEvent = new NpcShopBuyEventArgs(buyItemPacket.ItemId, buyItemPacket.Quantity, buyItemPacket.Tab, buyItemPacket.Slot);

            client.Player.NotifySystem<NpcShopSystem>(npcShopEvent);
        }

        [PacketHandler(PacketType.SELLITEM)]
        public static void OnSellItem(WorldClient client, INetPacketStream packet)
        {
            var sellItemPacket = new SellItemPacket(packet);
            var npcShopEvent = new NpcShopSellEventArgs(sellItemPacket.ItemUniqueId, sellItemPacket.Quantity);

            client.Player.NotifySystem<NpcShopSystem>(npcShopEvent);
        }
    }
}
