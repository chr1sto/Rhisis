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
    }
}
