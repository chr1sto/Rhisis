using Sylver.Network.Server;

namespace Rhisis.Cluster.Client
{
    public interface IClusterClient : INetServerClient
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
        /// Disconnects the current cluster client.
        /// </summary>
        void Disconnect();
    }
}
