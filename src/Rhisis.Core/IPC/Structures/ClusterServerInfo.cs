using System.Collections.Generic;

namespace Rhisis.Core.IPC.Structures
{
    public class ClusterServerInfo : BaseServerInfo
    {
        public List<WorldServerInfo> Worlds { get; private set; }

        public ClusterServerInfo(int id, string host, string name)
            : base(id, host, name)
        {
            this.Worlds = new List<WorldServerInfo>();
        }
    }
}
