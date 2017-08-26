using Ether.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Ether.Network.Packets;
using Rhisis.Core.Network;
using Rhisis.Core.IO;

namespace Rhisis.Cluster
{
    public sealed class ClusterServer : NetServer<ClusterClient>
    {
        public ClusterServer()
        {

        }

        protected override void Initialize()
        {
        }

        protected override void OnClientConnected(ClusterClient connection)
        {
            Logger.Info("New client connected: {0}", connection.Id);
        }

        protected override void OnClientDisconnected(ClusterClient connection)
        {
            Logger.Info("Client {0} disconnected.", connection.Id);
        }

        protected override IReadOnlyCollection<NetPacketBase> SplitPackets(byte[] buffer)
        {
            return FFPacket.SplitPackets(buffer);
        }
    }

    public sealed class ClusterClient : NetConnection
    {
        public override void HandleMessage(NetPacketBase packet)
        {
            throw new NotImplementedException();
        }
    }
}
