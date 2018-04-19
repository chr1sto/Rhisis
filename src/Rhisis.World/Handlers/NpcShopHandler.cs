using Ether.Network.Packets;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.Core.Network.Packets.World;
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

            client.Player.Context.NotifySystem<NpcShopSystem>(client.Player, npcEvent);
        }

        [PacketHandler(PacketType.CLOSESHOPWND)]
        public static void OnCloseShopWindow(WorldClient client, INetPacketStream packet)
        {
            client.Player.Context.NotifySystem<NpcShopSystem>(client.Player, new NpcShopCloseEventArgs());
        }

        [PacketHandler(PacketType.BUYITEM)]
        public static void OnBuyItem(WorldClient client, INetPacketStream packet)
        {
            var buyItemPacket = new BuyItemPacket(packet);
            var npcShopEvent = new NpcShopBuyEventArgs(buyItemPacket.ItemId, buyItemPacket.Quantity, buyItemPacket.Tab, buyItemPacket.Slot);

            client.Player.Context.NotifySystem<NpcShopSystem>(client.Player, npcShopEvent);
        }

        [PacketHandler(PacketType.SELLITEM)]
        public static void OnSellItem(WorldClient client, INetPacketStream packet)
        {
            var sellItemPacket = new SellItemPacket(packet);
            var npcShopEvent = new NpcShopSellEventArgs(sellItemPacket.ItemUniqueId, sellItemPacket.Quantity);

            client.Player.Context.NotifySystem<NpcShopSystem>(client.Player, npcShopEvent);
        }
    }
}
