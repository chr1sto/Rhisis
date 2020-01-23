using System.Collections.Generic;

namespace Rhisis.Network.ISC.Structures
{
    public class ClusterServerInfo : BaseServerInfo
    {
        public List<WorldServerInfo> WorldServers { get; private set; }

        public ClusterServerInfo(int id, string host, string name)
            : base(id, host, name)
        {
            WorldServers = new List<WorldServerInfo>();
        }
    }
}
