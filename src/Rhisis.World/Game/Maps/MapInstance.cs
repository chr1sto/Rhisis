using Rhisis.Core.Resources;
using Rhisis.Core.Structures;
using Rhisis.World.Game.Factories;
using Rhisis.World.Game.Maps.Regions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Rhisis.World.Game.Maps
{
    [DebuggerDisplay("{Name}")]
    public class MapInstance : MapContext, IMapInstance
    {
        public const int DefaultMapLayerId = 1;
        public const int MapLandSize = 128;
        public const int FrameRate = 15;
        public static readonly float UpdateRate = 1000 / FrameRate;

        private readonly ConcurrentDictionary<int, IMapLayer> _layers;
        private readonly IMapFactory _mapFactory;

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public WldFileInformations MapInformation { get; }

        /// <inheritdoc />
        public IMapLayer DefaultMapLayer { get; private set; }

        /// <inheritdoc />
        public IMapRevivalRegion DefaultRevivalRegion { get; private set; }

        /// <inheritdoc />
        public int Width => MapInformation.Width;

        /// <inheritdoc />
        public int Length => MapInformation.Length;

        /// <inheritdoc />
        public IEnumerable<IMapLayer> Layers => _layers.Values;

        /// <inheritdoc />
        public IEnumerable<IMapRegion> Regions { get; private set; }

        /// <summary>
        /// Creates a new <see cref="MapInstance"/>.
        /// </summary>
        /// <param name="id">Map Id.</param>
        /// <param name="name">Map name.</param>
        /// <param name="worldInformations">Map world informations.</param>
        /// <param name="mapFactory">Map factory.</param>
        public MapInstance(int id, string name, WldFileInformations worldInformations, IMapFactory mapFactory)
        {
            Id = id;
            Name = name;
            MapInformation = worldInformations;
            _mapFactory = mapFactory;
            _layers = new ConcurrentDictionary<int, IMapLayer>();
        }

        // <inheritdoc />
        public IMapLayer CreateMapLayer()
        {
            int layerId = _layers.Count > 0 ? _layers.Values.Max(x => x.Id) + 1 : DefaultMapLayerId;

            return CreateMapLayer(layerId);
        }

        // <inheritdoc />
        public IMapLayer CreateMapLayer(int id)
        {
            var mapLayer = _mapFactory.CreateLayer(this, id);

            _layers.TryAdd(id, mapLayer);

            if (DefaultMapLayer == null)
                DefaultMapLayer = mapLayer;

            return mapLayer;
        }

        // <inheritdoc />
        public void DeleteMapLayer(int id)
        {
            throw new NotImplementedException();
        }

        // <inheritdoc />
        public IMapLayer GetMapLayer(int id) => _layers.TryGetValue(id, out IMapLayer layer) ? layer : null;

        // <inheritdoc />
        public IMapRevivalRegion GetNearRevivalRegion(Vector3 position) => GetNearRevivalRegion(position, false);

        /// <inheritdoc />
        public IMapRevivalRegion GetNearRevivalRegion(Vector3 position, bool isChaoMode)
        {
            IEnumerable<IMapRevivalRegion> revivalRegions = Regions.Where(x => x is IMapRevivalRegion).Cast<IMapRevivalRegion>();
            var nearestRevivalRegion = revivalRegions.FirstOrDefault(x => x.MapId == Id && x.IsChaoRegion == isChaoMode && x.Contains(position) && x.TargetRevivalKey);

            if (nearestRevivalRegion != null)
                return GetRevivalRegion(nearestRevivalRegion.Key, isChaoMode);

            revivalRegions = from x in Regions
                             where x is IMapRevivalRegion y && y.IsChaoRegion == isChaoMode && !y.TargetRevivalKey
                             let region = x as IMapRevivalRegion
                             let distance = position.GetDistance3D(region.RevivalPosition)
                             orderby distance ascending
                             select region;

            return revivalRegions.FirstOrDefault() ?? DefaultRevivalRegion;
        }

        /// <inheritdoc />
        public IMapRevivalRegion GetRevivalRegion(string revivalKey) => GetRevivalRegion(revivalKey, false);

        /// <inheritdoc />
        public IMapRevivalRegion GetRevivalRegion(string revivalKey, bool isChaoMode)
        {
            IEnumerable<IMapRevivalRegion> revivalRegions = Regions.Where(x => x is IMapRevivalRegion).Cast<IMapRevivalRegion>();
            IEnumerable<IMapRevivalRegion> revivalRegion = from x in revivalRegions
                                                           where x.Key.Equals(revivalKey, StringComparison.OrdinalIgnoreCase) && x.IsChaoRegion == isChaoMode && !x.TargetRevivalKey
                                                           select x;

            return revivalRegion.FirstOrDefault() ?? DefaultRevivalRegion;
        }

        /// <inheritdoc />
        public bool ContainsPosition(Vector3 position)
        {
            float x = position.X / MapInformation.MPU;
            float z = position.Z / MapInformation.MPU;

            if (x < 0 || x > Width * MapLandSize || z < 0 || z > Length * MapLandSize)
                return false;

            return true;
        }

        /// <summary>
        /// Sets the map regions.
        /// </summary>
        /// <param name="regions">Map regions.</param>
        internal void SetRegions(IEnumerable<IMapRegion> regions)
        {
            if (!regions.Any(x => x is IMapRevivalRegion))
            {
                // Loads the default revival region if no revival region is loaded.
                DefaultRevivalRegion = new MapRevivalRegion(0, 0, 0, 0,
                    MapInformation.RevivalMapId, MapInformation.RevivalKey, null, false, false);
            }

            Regions = new List<IMapRegion>(regions);
        }

        /// <inheritdoc />
        public override string ToString() => Name;

        /// <inheritdoc />
        public void Dispose()
        {
            _layers.Clear();
        }
    }
}
