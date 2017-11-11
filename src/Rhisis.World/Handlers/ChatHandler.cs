using Ether.Network.Packets;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.World.Systems;

namespace Rhisis.World.Handlers
{
    public static class ChatHandler
    {
        [PacketHandler(PacketType.CHAT)]
        public static void OnChat(WorldClient client, NetPacketBase packet)
        {
            var message = packet.Read<string>();
            var chatEvent = new ChatEventArgs(message);

            client.Player.Context.NotifySystem<ChatSystem>(client.Player, chatEvent);
        }
    }
}
