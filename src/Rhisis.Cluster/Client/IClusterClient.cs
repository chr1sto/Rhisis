using Ether.Network.Common;

namespace Rhisis.Cluster.Client
{
    public interface IClusterClient : INetUser
    {
        /// <summary>
        /// Gets the ID assigned to this session.
        /// </summary>
        uint SessionId { get; }

        /// <summary>
        /// Gets or sets the Login protect value. 
        /// This value is random and valid only for this session in order to secure num pad disposition.
        /// </summary>
        int LoginProtectValue { get; set; }

        /// <summary>
        /// Gets the remote end point (IP and port) for this client.
        /// </summary>
        string RemoteEndPoint { get; }

        /// <summary>
        /// Disconnects the current cluster client.
        /// </summary>
        void Disconnect();
    }
}
