using Ether.Network.Packets;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.Core.Network.Packets.World;
using Rhisis.World.Systems.NpcShop;

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

        [PacketHandler(PacketType.BUYITEM)]
        public static void OnBuyItem(WorldClient client, INetPacketStream packet)
        {
            var buyItemPacket = new BuyItemPacket(packet);
            var npcShopEvent = new NpcShopBuyItemEventArgs(buyItemPacket.ItemId, buyItemPacket.Quantity);

            client.Player.Context.NotifySystem<NpcShopSystem>(client.Player, npcShopEvent);
        }

        [PacketHandler(PacketType.SELLITEM)]
        public static void OnSellItem(WorldClient client, INetPacketStream packet)
        {
            var sellItemPacket = new SellItemPacket(packet);
            var npcShopEvent = new NpcShopSellItemEventArgs(sellItemPacket.ItemUniqueId, sellItemPacket.Quantity);

            client.Player.Context.NotifySystem<NpcShopSystem>(client.Player, npcShopEvent);
        }
    }
}
