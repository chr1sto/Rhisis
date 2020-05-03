using System.Collections.Generic;

namespace Rhisis.Network.Core
{
    /// <summary>
    /// Defines the server types.
    /// </summary>
    public enum ServerType
    {
        /// <summary>
        /// Unknown server type.
        /// </summary>
        Unknown,

        /// <summary>
        /// Cluster Server.
        /// </summary>
        Cluster,

        /// <summary>
        /// World Server.
        /// </summary>
        World
    }

    public abstract class ServerInfo
    {
        /// <summary>
        /// Gets or sets the server's unique Id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets or sets the server's name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the server's host name or IP address.
        /// </summary>
        public string Host { get; }

        /// <summary>
        /// Gets or sets the server's listening port.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Creates and initializes a new <see cref="ServerInfo"/> instance.
        /// </summary>
        /// <param name="id">Server unique Id.</param>
        /// <param name="name">Server name.</param>
        /// <param name="host">Server host name or IP address.</param>
        /// <param name="port">Server listening port.</param>
        protected ServerInfo(int id, string name, string host, int port)
        {
            Id = id;
            Name = name;
            Host = host;
            Port = port;
        }
    }

    public sealed class ClusterServerInfo : ServerInfo
    {
        /// <summary>
        /// Gets the list of all world servers of the current cluster.
        /// </summary>
        public List<WorldServerInfo> Worlds { get; }

        /// <summary>
        /// Creates a new <see cref="ClusterServerInfo"/> instance.
        /// </summary>
        /// <param name="id">Cluster server unique id.</param>
        /// <param name="name">Cluster server name.</param>
        /// <param name="host">Cluster server host name or IP address.</param>
        /// <param name="port">Cluster server port.</param>
        public ClusterServerInfo(int id, string name, string host, int port) 
            : base(id, name, host, port)
        {
            Worlds = new List<WorldServerInfo>();
        }
    }
    public sealed class WorldServerInfo : ServerInfo
    {
        /// <summary>
        /// Gets or sets the world server's parent cluster id.
        /// </summary>
        public int ParentClusterId { get; }

        /// <summary>
        /// Creates a new <see cref="WorldServerInfo"/> instance.
        /// </summary>
        /// <param name="id">World server unique id.</param>
        /// <param name="name">World server name.</param>
        /// <param name="host">World server host name or IP address.</param>
        /// <param name="port">World server port.</param>
        /// <param name="parentClusterId">World server parent cluster server id.</param>
        public WorldServerInfo(int id, string name, string host, int port, int parentClusterId) 
            : base(id, name, host, port)
        {
            ParentClusterId = parentClusterId;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is WorldServerInfo wsi && Id.Equals(wsi.Id);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
