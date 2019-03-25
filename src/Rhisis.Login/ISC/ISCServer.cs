using Ether.Network.Server;
using NLog;
using Rhisis.Network.ISC;
using Rhisis.Network.ISC.Structures;
using Rhisis.Network;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Login.ISC.Packets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Login.ISC
{
    public sealed class ISCServer : NetServer<ISCClient>
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets the list of the connected clusters.
        /// </summary>
        public IEnumerable<ClusterServerInfo> ClusterServers => from x in this.Clients
                                                                where x.ServerInfo is ClusterServerInfo
                                                                select x.ServerInfo as ClusterServerInfo;

        /// <summary>
        /// Creates a new <see cref="ISCServer"/> instance.
        /// </summary>
        /// <param name="configuration"></param>
        public ISCServer(ISCConfiguration configuration)
        {
            this.Configuration.Host = configuration.Host;
            this.Configuration.Port = configuration.Port;
            this.Configuration.MaximumNumberOfConnections = 100;
            this.Configuration.Backlog = 100;
            this.Configuration.BufferSize = 1024;
            this.Configuration.Blocking = false;

            Logger.Trace("ISC config -> Host: {0}, Port: {1}, MaxNumberOfConnections: {2}, Backlog: {3}, BufferSize: {4}",
                this.Configuration.Host,
                this.Configuration.Port,
                this.Configuration.MaximumNumberOfConnections,
                this.Configuration.Backlog,
                this.Configuration.BufferSize);
        }

        /// <inheritdoc />
        protected override void Initialize()
        {
            PacketHandler<ISCClient>.Initialize();
            Logger.Info("ISC server is started and listen on {0}:{1}.", this.Configuration.Host, this.Configuration.Port);
        }

        /// <inheritdoc />
        protected override void OnClientConnected(ISCClient client)
        {
            client.Initialize(this);
            Logger.Debug("New incoming ISC client connection from {0}.", client.RemoteEndPoint);
            ISCPacketFactory.SendWelcome(client);
        }

        /// <inheritdoc />
        protected override void OnClientDisconnected(ISCClient client)
        {
            var worldInfo = client.ServerInfo as WorldServerInfo;
            var clusterInfo = client.ServerInfo as ClusterServerInfo;

            if (client.ServerInfo == null)
                Logger.Debug("ISC client disconnected from {0}.", client.RemoteEndPoint);
            else if (worldInfo != null)
            {
                Logger.Info("World server '{0}' of cluster '{1}' disconnected from ISC server.", worldInfo.Name, 
                    this.GetCluster(worldInfo.ParentClusterId)?.ServerInfo.Name ?? $"Id: {worldInfo.ParentClusterId.ToString()}");
            }
            else if (clusterInfo != null)
                Logger.Info("Cluster server '{0}' disconnected from ISC server.", clusterInfo.Name);              

            client.Disconnect();
        }

        /// <inheritdoc />
        protected override void OnError(Exception exception)
        {
            Logger.Error("ISC socket error: {0}", exception.Message);
        }

        /// <summary>
        /// Check if a cluster is connected.
        /// </summary>
        /// <param name="id">Cluster id</param>
        /// <returns></returns>
        internal bool HasClusterWithId(int id)
        {
            return this.Clients.Any(x => x.ServerInfo is ClusterServerInfo && x.ServerInfo.Id == id);
        }

        /// <summary>
        /// Gets the cluster by his id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal ISCClient GetCluster(int id)
        {
            return (from x in this.Clients
                    where x.ServerInfo is ClusterServerInfo
                    where x.ServerInfo.Id == id
                    select x).FirstOrDefault();
        }

        /// <summary>
        /// Check if a World is inside a cluster.
        /// </summary>
        /// <param name="clusterId">Cluster Id</param>
        /// <param name="worldId">World Id</param>
        /// <returns></returns>
        internal bool HasWorldInCluster(int clusterId, int worldId)
        {
            ISCClient cluster = this.GetCluster(clusterId);

            if (cluster == null)
                return false;

            var clusterInfo = cluster.GetServerInfo<ClusterServerInfo>();

            return clusterInfo.WorldServers.Any(x => x.Id == worldId);
        }

        /// <summary>
        /// Gets a world by his id and parent cluster id.
        /// </summary>
        /// <param name="parentClusterId">Parent cluster Id</param>
        /// <param name="worldId">World id</param>
        /// <returns></returns>
        internal ISCClient GetWorld(int parentClusterId, int worldId)
        {
            return (from x in this.Clients
                    where x.Type == ISCServerType.World
                    let worldInfo = x.ServerInfo as WorldServerInfo
                    where worldInfo.ParentClusterId == parentClusterId
                    where worldInfo.Id == worldId
                    select x).FirstOrDefault();
        }
    }
}
