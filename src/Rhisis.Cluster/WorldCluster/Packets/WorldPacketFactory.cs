using Rhisis.Cluster.WorldCluster.Server;
using Rhisis.Network.Core;
using Sylver.Network.Data;

namespace Rhisis.Cluster.WorldCluster.Packets
{
    public class WorldPacketFactory : IWorldPacketFactory
    {
        /// <inheritdoc />
        public void SendHandshake(IWorldClusterServerClient client)
        {
            using (var packet = new NetPacket())
            {
                packet.Write((uint)CorePacketType.Welcome);
                client.Send(packet);
            }
        }

        /// <inheritdoc />
        public void SendAuthenticationResult(IWorldClusterServerClient client, CoreAuthenticationResultType authenticationResultType)
        {
            using (var packet = new NetPacket())
            {
                packet.Write((uint)CorePacketType.AuthenticationResult);
                packet.Write((uint)authenticationResultType);
                client.Send(packet);
            }
        }
    }
}
