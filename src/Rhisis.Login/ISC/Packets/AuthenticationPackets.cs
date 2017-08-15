using Ether.Network.Packets;
using Rhisis.Core.ISC.Packets;

namespace Rhisis.Login.ISC.Packets
{
    public static partial class PacketFactory
    {
        public static void SendWelcome(InterClient client)
        {
            using (var packet = new NetPacket())
            {
                packet.Write((uint)InterPacketType.WELCOME);

                client.Send(packet);
            }
        }

        public static void SendAuthenticationResult(InterClient client, InterServerError error)
        {
            using (var packet = new NetPacket())
            {
                packet.Write((uint)InterPacketType.AUTHENTICATION_RESULT);
                packet.Write((uint)error);

                client.Send(packet);
            }
        }
    }
}
