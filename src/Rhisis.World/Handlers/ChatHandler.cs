using Ether.Network.Packets;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.Core.Network.Packets.World;
using Rhisis.World.Systems;
using Rhisis.World.Systems.Events;

namespace Rhisis.World.Handlers
{
    public static class ChatHandler
    {
        [PacketHandler(PacketType.CHAT)]
        public static void OnChat(WorldClient client, NetPacketBase packet)
        {
            var chatPacket = new ChatPacket(packet);
            var chatEvent = new ChatEventArgs(chatPacket.Message);

            client.Player.Context.NotifySystem<ChatSystem>(client.Player, chatEvent);
        }
    }
}
