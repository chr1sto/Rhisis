using Ether.Network.Packets;
using Rhisis.Core.IPC;
using Rhisis.Core.IPC.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.Cluster.IPC
{
    public static class IPCPackets
    {
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
