using Microsoft.Extensions.Caching.Memory;
using Rhisis.Core.Structures.Game;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Rhisis.Core.Resources
{
    public class NewGameResources : IGameResources
    {
        private readonly IMemoryCache _cache;
        private ConcurrentDictionary<int, MoverData> _movers;
        private ConcurrentDictionary<int, ItemData> _items;

        public IDictionary<int, MoverData> Movers
        {
            get
            {
                if (this._movers == null)
                {
                    this._cache.TryGetValue(nameof(MoverData), out this._movers);
                }

                return this._movers;
            }
        }

        public IDictionary<int, ItemData> Items
        {
            get
            {
                if (this._items == null)
                {
                    this._cache.TryGetValue(nameof(ItemData), out this._items);
                }

                return this._items;
            }
        }

        public NewGameResources(IMemoryCache cache)
        {
            this._cache = cache;
        }

        public void Load()
        {
            // TODO
        }
    }
}
