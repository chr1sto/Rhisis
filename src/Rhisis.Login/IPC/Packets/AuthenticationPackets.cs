using Ether.Network;
using Ether.Network.Packets;
using Rhisis.Core.IPC.Packets;
using Rhisis.Core.IPC.Structures;

namespace Rhisis.Login.IPC.Packets
{
    public static partial class PacketFactory
    {
        public static void SendWelcome(NetConnection client)
        {
            using (var packet = new NetPacket())
            {
                packet.Write((uint)InterPacketType.Welcome);

                client.Send(packet);
            }
        }

        public static void SendAuthenticationResult(NetConnection client, InterServerError error)
        {
            using (var packet = new NetPacket())
            {
                packet.Write((uint)InterPacketType.AuthenticationResult);
                packet.Write((uint)error);

                client.Send(packet);
            }
        }
    }
}
