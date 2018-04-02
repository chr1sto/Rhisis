using Rhisis.World.Game.Core.Interfaces;
using System.Collections.Generic;

namespace Rhisis.World.Game.Maps
{
    /// <summary>
    /// Describes the behavior of a Map instance.
    /// </summary>
    public interface IMapInstance : IContext
    {
        /// <summary>
        /// Gets the map id.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Gets the map name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the map layers.
        /// </summary>
        IList<IMapLayer> Layers { get; }

        /// <summary>
        /// Load NPC from the DYO file.
        /// </summary>
        void LoadDyo();

        /// <summary>
        /// Load regions from the RGN file.
        /// </summary>
        void LoadRgn();
    }
}
