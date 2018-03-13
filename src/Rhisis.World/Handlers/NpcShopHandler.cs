using Ether.Network.Packets;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.Core.Network.Packets.World;

namespace Rhisis.World.Handlers
{
    public static class NpcShopHandler
    {
        [PacketHandler(PacketType.OPENSHOPWND)]
        public static void OnOpenShopWindow(WorldClient client, NetPacketBase packet)
        {
            var openShopPacket = new OpenShopWindowPacket(packet);

            // TODO: Get the correct NPC from the global context
            // TODO: Send NPC shop to client
        }
    }
}
