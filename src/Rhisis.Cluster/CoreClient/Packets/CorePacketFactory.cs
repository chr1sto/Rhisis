using Rhisis.Core.Structures.Configuration;
using Rhisis.Network.Core;
using Sylver.Network.Data;

namespace Rhisis.Cluster.CoreClient.Packets
{
    public class CorePacketFactory : ICorePacketFactory
    {
        /// <inheritdoc />
        public void SendAuthentication(IClusterCoreClient client, ClusterConfiguration clusterConfiguration)
        {
            using (var packet = new NetPacket())
            {
                packet.Write((uint)CorePacketType.Authenticate);
                packet.Write(clusterConfiguration.Id);
                packet.Write(clusterConfiguration.Name);
                packet.Write(clusterConfiguration.Host);
                packet.Write(clusterConfiguration.Port);
                packet.Write((byte)ServerType.Cluster);

                client.Send(packet);
            }
        }
    }
}
