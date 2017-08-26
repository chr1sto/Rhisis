using Ether.Network;
using System;
using Ether.Network.Packets;

namespace Rhisis.Cluster
{
    public sealed class ClusterClient : NetConnection
    {
        public override void HandleMessage(NetPacketBase packet)
        {
            throw new NotImplementedException();
        }
    }
}
