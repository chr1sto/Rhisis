using System.Collections.Generic;
using Rhisis.Network.Core;

namespace Rhisis.Cluster.Models
{
    public interface IWorldCache: IList<WorldServerInfo>
    {
        WorldServerInfo GetById(int id);
    }
}