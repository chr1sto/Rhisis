using System.Collections.Generic;

namespace Rhisis.Core.Resources
{
    public interface ICache<in TK, T>
    {
        /// <summary>
        /// Adds an item to the cache
        /// </summary>
        /// <param name="key">Unique key for the item</param>
        /// <param name="item">Actual item</param>
        /// <returns></returns>
        bool Add(TK key, T item);
        
        /// <summary>
        /// Remove item from cache
        /// </summary>
        /// <param name="key">Unique to remove coupled item</param>
        /// <returns>true if successfully removed</returns>
        bool Remove(TK key);
        
        /// <summary>
        /// Try and get the item with given unique key
        /// </summary>
        /// <param name="key">Unique key to get the item with</param>
        /// <returns>Item or default value if not found</returns>
        T TryGetOrDefault(TK key);
        
        ICollection<T> Items { get; }
    }
}