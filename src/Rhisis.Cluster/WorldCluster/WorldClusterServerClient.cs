using System.Net.Sockets;
using Sylver.Network.Data;
using Sylver.Network.Server;

namespace Rhisis.Cluster.WorldCluster
{
    public class WorldClusterServerClient : NetServerClient, IWorldClusterServerClient
    {
        public WorldClusterServerClient(Socket socketConnection) : base(socketConnection)
        {
        }

        public override void HandleMessage(INetPacketStream packet)
        {
            throw new System.NotImplementedException();
        }
    }
}