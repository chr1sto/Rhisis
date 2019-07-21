using Ether.Network.Common;
using Ether.Network.Packets;
using Rhisis.Network.ISC;
using Rhisis.Network.ISC.Packets;

namespace Rhisis.Cluster.ISC
{
    public static class ISCPacketFactory
    {
        /// <summary>
        /// Send an authentication request to the ISCServer.
        /// </summary>
        /// <param name="client">client connection</param>
        /// <param name="id">Server Id</param>
        /// <param name="host">Server Host</param>
        /// <param name="name">Server name</param>
        public static void SendAuthentication(INetUser client, int id, string host, string name, int port)
        {
            using (var packet = new NetPacket())
            {
                packet.Write((uint)ISCPacketType.AUTHENT);
                packet.Write(id);
                packet.Write(name);
                packet.Write(host);
                packet.Write(port);
                packet.Write((byte)ISCServerType.Cluster);

                client.Send(packet);
            }
        }
    }
}
