using Rhisis.World.Game.Core;
using Rhisis.World.Game.Maps.Regions;
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
        IReadOnlyList<IMapRegion> Regions { get; }

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
        /// Gets a map layer by his id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IMapLayer GetMapLayer(int id);

        /// <summary>
        /// Gets the default map layer.
        /// </summary>
        /// <returns></returns>
        IMapLayer GetDefaultMapLayer();

        /// <summary>
        /// Deletes a map layer by his id.
        /// </summary>
        /// <param name="id"></param>
        void DeleteMapLayer(int id);

        /// <summary>
        /// Starts a context in a parallel task.
        /// </summary>
        void StartUpdateTask();

        /// <summary>
        /// Stops the context and the task.
        /// </summary>
        void StopUpdateTask();
    }
}
