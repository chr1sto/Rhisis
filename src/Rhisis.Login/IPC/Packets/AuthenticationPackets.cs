using Ether.Network.Packets;
using Rhisis.Core.IPC.Packets;

namespace Rhisis.Login.IPC.Packets
{
    public static partial class PacketFactory
    {
        public static void SendWelcome(IPCClient client)
        {
            using (var packet = new NetPacket())
            {
                packet.Write((uint)InterPacketType.WELCOME);

                client.Send(packet);
            }
        }

        public static void SendAuthenticationResult(IPCClient client, InterServerError error)
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
