using Rhisis.Cluster.Client;
using Rhisis.Cluster.Packets;
using Rhisis.Core.Handlers.Attributes;
using Rhisis.Network.Packets;
using Rhisis.Network.Packets.Cluster;

namespace Rhisis.Cluster.Handlers
{
    public class CommonClusterHandler
    {
        private readonly IClusterPacketFactory _clusterPacketFactory;

        public CommonClusterHandler(IClusterPacketFactory clusterPacketFactory)
        {
            this._clusterPacketFactory = clusterPacketFactory;
        }

        [HandlerAction(PacketType.PING)]
        public void OnPing(ClusterClient client, PingPacket pingPacket)
        {
            if (!pingPacket.IsTimeOut)
                this._clusterPacketFactory.SendPong(client, pingPacket.Time);
        }

        [HandlerAction(PacketType.QUERYTICKCOUNT)]
        public void OnQueryTickCount(ClusterClient client, QueryTickCountPacket packet)
        {
            this._clusterPacketFactory.SendQueryTickCount(client, packet.Time);
        }
    }
}
