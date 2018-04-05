using Rhisis.World.Game.Core.Interfaces;
using Rhisis.World.Game.Regions;
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
        IReadOnlyList<IMapLayer> Layers { get; }

        /// <summary>
        /// Gets the map regions.
        /// </summary>
        IReadOnlyList<IRegion> Regions { get; }

        /// <summary>
        /// Load NPC from the DYO file.
        /// </summary>
        void LoadDyo();

        /// <summary>
        /// Load regions from the RGN file.
        /// </summary>
        void LoadRgn();

        /// <summary>
        /// Creates a new map layer and gives it a random id.
        /// </summary>
        /// <returns></returns>
        IMapLayer CreateMapLayer();

        /// <summary>
        /// Creates a new map layer with an id.
        /// </summary>
        /// <param name="id">Map layer id</param>
        /// <returns></returns>
        IMapLayer CreateMapLayer(int id);

        /// <summary>
        /// Creates a new map layer instance with an id.
        /// </summary>
        /// <param name="id">Map layer instance id</param>
        /// <returns></returns>
        IMapLayerInstance CreaMapLayerInstance(int id);

        /// <summary>
        /// Gets a map layer by his id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IMapLayer GetMapLayer(int id);

        /// <summary>
        /// Deletes a map layer by his id.
        /// </summary>
        /// <param name="id"></param>
        void DeleteMapLayer(int id);
    }
}
