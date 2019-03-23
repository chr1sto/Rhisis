using Rhisis.World.Game.Core;
using Rhisis.World.Game.Maps.Regions;
using System.Collections.Generic;

namespace Rhisis.World.Game.Maps
{
    /// <summary>
    /// Describes the behavior of a Map layer
    /// </summary>
    public interface IMapLayer : IContext
    {
        /// <summary>
        /// Gets the map layer id.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Gets the parent map of the current layer.
        /// </summary>
        IMapInstance Parent { get; }

        /// <summary>
        /// Gets the regions of the current layer.
        /// </summary>
        ICollection<IMapRegion> Regions { get; }
    }
}
