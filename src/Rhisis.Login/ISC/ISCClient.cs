using Ether.Network.Common;
using Ether.Network.Packets;
using NLog;
using Rhisis.Core.Exceptions;
using Rhisis.Core.ISC;
using Rhisis.Core.ISC.Packets;
using Rhisis.Core.ISC.Structures;
using Rhisis.Core.Network;
using Rhisis.Login.ISC.Packets;
using System.Collections.Generic;

namespace Rhisis.Login.ISC
{
    public sealed class ISCClient : NetUser
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private ISCServer _server;
        
        public InterServerType Type { get; internal set; }

        public BaseServerInfo ServerInfo { get; internal set; }

        public ISCServer IcsServer => this._server;

        public void Initialize(ISCServer server)
        {
            this._server = server;
            PacketFactory.SendWelcome(this);
        }

        public override void HandleMessage(INetPacketStream packet)
        {
            var packetHeaderNumber = packet.Read<uint>();

            try
            {
                PacketHandler<ISCClient>.Invoke(this, packet, (InterPacketType)packetHeaderNumber);
            }
            catch (KeyNotFoundException)
            {
                Logger.Warn("Unknown ISC packet with header: 0x{0}", packetHeaderNumber.ToString("X2"));
            }
            catch (RhisisPacketException packetException)
            {
                Logger.Error(packetException.Message);
#if DEBUG
                Logger.Debug("STACK TRACE");
                Logger.Debug(packetException.InnerException?.StackTrace);
#endif
            }
        }

        public void Disconnect()
        {
            switch (this.Type)
            {
                case InterServerType.Cluster:
                    (this.ServerInfo as ClusterServerInfo).Worlds.Clear();
                    break;
                case InterServerType.World:
                    var worldInfo = this.GetServerInfo<WorldServerInfo>();
                    ISCClient cluster = this._server.GetCluster(worldInfo.ParentClusterId);
                    var clusterInfo = cluster?.GetServerInfo<ClusterServerInfo>();

                    if (clusterInfo == null)
                    {
                        Logger.Warn("Cannot find parent cluster of world server : {0}", worldInfo.Name);
                        return;
                    }

                    clusterInfo.Worlds.Remove(worldInfo);
                    PacketFactory.SendUpdateWorldList(cluster, clusterInfo.Worlds);
                    break;
            }
        }

        public T GetServerInfo<T>() where T : BaseServerInfo
        {
            return this.ServerInfo as T;
        }
    }
}
