using Ether.Network.Server;
using Rhisis.Network.ISC.Structures;

namespace Rhisis.Cluster
{
    public interface IClusterServer : INetServer
    {
        /// <summary>
        /// Gets world server by his id.
        /// </summary>
        /// <param name="id">World Server id</param>
        /// <returns></returns>
        WorldServerInfo GetWorldServerById(int id);
    }
}