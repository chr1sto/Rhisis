using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.DependencyInjection;
using Rhisis.Network.Core;
using Sylver.Network.Data;

namespace Rhisis.World.ClusterClient.Packets
{
    [Injectable(ServiceLifetime.Singleton)]
    public sealed class ClusterPacketFactory : IClusterPacketFactory
    {
        /// <inheritdoc />
        public void SendAuthentication(IWorldClusterClient client)
        {
            using (var packet = new NetPacket())
            {
                var config = client.WorldServerConfiguration;
                packet.Write((uint)WorldClusterPacketType.Authenticate);
                packet.Write(config.Id);
                packet.Write(config.Name);
                packet.Write(config.Host);
                packet.Write(config.Port);
                packet.Write(config.ClusterId);
                packet.Write(client.WorldClusterClientConfiguration.Password);
                client.Send(packet);
            }
        }
    }
}
