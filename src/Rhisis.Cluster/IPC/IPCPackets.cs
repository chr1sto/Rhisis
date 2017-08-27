using Ether.Network.Packets;
using Rhisis.Core.IPC;
using Rhisis.Core.IPC.Packets;

namespace Rhisis.Cluster.IPC
{
    public static class IPCPackets
    {
        /// <summary>
        /// Send an authentication request to the IPCServer
        /// </summary>
        /// <param name="client">IPC Client</param>
        /// <param name="id">Server Id</param>
        /// <param name="host">Server Host</param>
        /// <param name="name">Server name</param>
        public static void SendAuthentication(IPCClient client, int id, string host, string name)
        {
            using (var packet = new NetPacket())
            {
                packet.Write((uint)InterPacketType.AUTHENTICATE);
                packet.Write(id);
                packet.Write(host);
                packet.Write(name);
                packet.Write((byte)InterServerType.Cluster);

                client.Send(packet);
            }
        }
    }
}
