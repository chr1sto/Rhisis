using System.Collections.Generic;
using System.Linq;
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
                packet.Write(client.CoreConfiguration.Password);

                client.Send(packet);
            }
        }
        
        /// <inheritdoc />
        public void SendUpdateWorldList(IClusterCoreClient client, IEnumerable<WorldServerInfo> worldServers)
        {
            using (var packet = new NetPacket())
            {
                packet.Write((uint)CorePacketType.UpdateClusterWorldsList);
                packet.Write(worldServers.Count());

                foreach (WorldServerInfo world in worldServers)
                {
                    packet.Write(world.Id);
                    packet.Write(world.Host);
                    packet.Write(world.Name);
                    packet.Write(world.Port);
                    packet.Write(world.ParentClusterId);
                }

                client.Send(packet);
            }
        }
    }
}
