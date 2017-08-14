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

        public static void SendLoginError(LoginClient client, ErrorType error)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.ERROR);
                packet.Write((int)error);

                client.Send(packet);
            }
        }

        public static void SendServerList(LoginClient client)
        {
        }
    }
}
