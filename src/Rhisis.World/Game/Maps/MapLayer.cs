using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Maps.Regions;
using Rhisis.World.Systems.Visibility;
using System.Collections.Generic;

namespace Rhisis.World.Game.Maps
{
    public sealed class MapLayer : MapContext, IMapLayer
    {
        private readonly IVisibilitySystem _visibilitySystem;

        /// <inheritdoc />
        public IMapInstance ParentMap { get; }

        /// <inheritdoc />
        public ICollection<IMapRegion> Regions { get; }

        /// <summary>
        /// Creates a new <see cref="MapLayer"/> instance.
        /// </summary>
        /// <param name="parentMapInstance">Parent map.</param>
        /// <param name="layerId">Layer id.</param>
        /// <param name="visibilitySystem">Visibility system.qvisual</param>
        public MapLayer(IMapInstance parentMapInstance, int layerId, IVisibilitySystem visibilitySystem)
        {
            Id = layerId;
            ParentMap = parentMapInstance;
            _visibilitySystem = visibilitySystem;
        }

        /// <inheritdoc />
        public override void DeleteEntity(IWorldEntity entityToDelete)
        {
            base.DeleteEntity(entityToDelete);
            _visibilitySystem.DespawnEntity(entityToDelete);
        }
    }
}
