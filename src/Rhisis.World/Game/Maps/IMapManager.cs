using Rhisis.Core.Resources;
using System.Collections.Generic;

namespace Rhisis.World.Game.Maps
{
    /// <summary>
    /// Provides a mechanism to manage all maps of a world server.
    /// </summary>
    public interface IMapManager : IGameResourceLoader
    {
        /// <summary>
        /// Gets a collection of all maps available on the server.
        /// </summary>
        IEnumerable<IMapInstance> Maps { get; }

        /// <summary>
        /// Gets a map instance by its id.
        /// </summary>
        /// <param name="id">Map id.</param>
        /// <returns>Map instance.</returns>
        IMapInstance GetMap(int id);
    }
}
