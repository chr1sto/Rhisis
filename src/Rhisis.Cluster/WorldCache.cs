using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using Rhisis.Core.Resources;
using Rhisis.Network.Core;

namespace Rhisis.Cluster
{
    public class WorldCache : ICache<int, WorldServerInfo>
    {
        private const string CacheKey = "ClusterWorlds";
        private readonly IMemoryCache _cache;
        
        private ConcurrentDictionary<int, WorldServerInfo> _worlds;
        
        public ICollection<WorldServerInfo> Items => GetCacheValue(CacheKey, ref _worlds).Values;

        public WorldCache(IMemoryCache cache)
        {
            _worlds = new ConcurrentDictionary<int, WorldServerInfo>();
            _cache = cache;
        }

        /// <inheritdoc />
        public bool Add(int key, WorldServerInfo item)
        {
            if (_worlds.ContainsKey(key)) 
                return false;
            
            _worlds[key] = item;
            _cache.Set(CacheKey, _worlds);
            return true;

        }

        /// <inheritdoc />
        public bool Remove(int key)
        {
            bool success = _worlds.Remove(key, out WorldServerInfo _);
            if (success)
                _cache.Set(CacheKey, _worlds);
            
            return success;
        }

        /// <inheritdoc />
        public WorldServerInfo TryGetOrDefault(int key)
        {
            return !_worlds.ContainsKey(key) ? null : _worlds[key];
        }

        private T GetCacheValue<T>(object key, ref T value)
        {
            if (Equals(value, default(T)))
            {
                _cache.TryGetValue(key, out value);
            }

            return value;
        }
    }
}