using Rhisis.Core.Common;
using Rhisis.World.Game.Entities;
using System.Collections.Generic;

namespace Rhisis.World.Game.Maps.Regions
{
    public interface IMapRespawnRegion : IMapRegion
    {
        /// <summary>
        /// Gets the respawn region object type.
        /// </summary>
        WorldObjectType ObjectType { get; }

        /// <summary>
        /// Gets the mover model id of the current respawn region.
        /// </summary>
        int ModelId { get; }

        /// <summary>
        /// Gets the repawn time of this region.
        /// </summary>
        int Time { get; }

        /// <summary>
        /// Gets the number of movers to spawn on this region.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the respawn basic height.
        /// </summary>
        float Height { get; }

        /// <summary>
        /// Gets a collection of the spawned entities on this region.
        /// </summary>
        IList<IWorldEntity> Entities { get; }
    }
}
