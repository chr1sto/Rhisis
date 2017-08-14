using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;

namespace Rhisis.Login.Packets
{
    public static class PacketFactory
    {
        public static void SendWelcome(LoginClient client, uint sessionId)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.WELCOME);
                packet.Write(sessionId);

                client.Send(packet);
            }
        }
    }
}
