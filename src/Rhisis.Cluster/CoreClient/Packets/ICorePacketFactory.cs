using Ether.Network.Packets;
using Rhisis.Network.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.Cluster.CoreClient.Packets
{
    public interface ICorePacketFactory
    {
        void SendAuthentication(IClusterCoreClient client);
    }

    public class CorePacketFactory : ICorePacketFactory
    {
        /// <inheritdoc />
        public void SendAuthentication(IClusterCoreClient client)
        {
            using (var packet = new NetPacket())
            {
                packet.Write((uint)CorePacketType.Authenticate);
                packet.Write(client.ClusterConfiguration.Id);
                packet.Write(client.ClusterConfiguration.Name);
                packet.Write(client.ClusterConfiguration.Host);
                packet.Write(client.ClusterConfiguration.Port);
                packet.Write((byte)ServerType.Cluster);

                client.Send(packet);
            }
        }
    }
}
