using Ether.Network.Common;

namespace Rhisis.Cluster.Client
{
    public interface IClusterClient : INetUser
    {
        /// <summary>
        /// Gets the ID assigned to this session.
        /// </summary>
        uint SessionId { get; }
    }
}
