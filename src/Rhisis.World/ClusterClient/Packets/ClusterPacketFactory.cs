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
                packet.Write((byte)ServerType.World);
                packet.Write(config.ClusterId);

                // TODO: add more information to packet if needed.
                client.Send(packet);
            }
        }
    }
}
