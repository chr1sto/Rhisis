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

            client.Player.Context.NotifySystem<NpcShopSystem>(client.Player, new NpcShopOpenEventArgs(openShopPacket.ObjectId));
        }

        [PacketHandler(PacketType.BUYITEM)]
        public static void OnBuyItem(WorldClient client, INetPacketStream packet)
        {
            var buyItemPacket = new BuyItemPacket(packet);

            client.Player.Context.NotifySystem<NpcShopSystem>(client.Player, new NpcShopBuyItemEventArgs(buyItemPacket.ItemId, buyItemPacket.Quantity));
        }
    }
}
