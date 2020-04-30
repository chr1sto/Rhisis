using System.Collections.Generic;
using System.Linq;
using Rhisis.Network.Core;

namespace Rhisis.Cluster.Models
{
    public class WorldCache : List<WorldServerInfo>, IWorldCache
    {
        private readonly object _mutate = new object();

        public new void Add(WorldServerInfo item)
        {
            lock (_mutate)
            {
                base.Add(item);
            }
        }

        public new bool Remove(WorldServerInfo item)
        {
            lock (_mutate)
            {
                return base.Remove(item);
            }
        }

        public new void Insert(int index, WorldServerInfo item)
        {
            lock (_mutate)
            {
                base.Insert(index, item);
            }
        }

        public new void RemoveAt(int index)
        {
            lock (_mutate)
            {
                base.RemoveAt(index);
            }
        }

        public WorldServerInfo GetById(int id)
        {
            lock (_mutate)
            {
                return this.SingleOrDefault(wsi => wsi.Id == id);
            }
        }
    }
}