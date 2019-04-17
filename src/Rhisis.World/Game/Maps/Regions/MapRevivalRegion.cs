using Rhisis.Core.Resources;
using Rhisis.Core.Structures;

namespace Rhisis.World.Game.Maps.Regions
{
    public class MapRevivalRegion : MapRegion, IMapRevivalRegion
    {
        /// <inheritdoc />
        public int MapId { get; }

        /// <inheritdoc />
        public string Key { get; }

        /// <inheritdoc />
        public bool IsChaoRegion { get; }

        /// <inheritdoc />
        public Vector3 RevivalPosition { get; }

        /// <summary>
        /// Creates a new <see cref="MapRevivalRegion"/> instance.
        /// </summary>
        /// <param name="x">Region X position on the world.</param>
        /// <param name="z">Region Z position on the world.</param>
        /// <param name="width">Region width.</param>
        /// <param name="length">Region length.</param>
        /// <param name="mapId">Revival map id.</param>
        /// <param name="key">Revival map key.</param>
        /// <param name="position">Revival position.</param>
        /// <param name="isChao">Indicates that the region is a revival region for PK players.</param>
        public MapRevivalRegion(int x, int z, int width, int length, int mapId, string key, Vector3 position, bool isChao)
            : base(x, z, width, length)
        {
            this.MapId = mapId;
            this.Key = key;
            this.RevivalPosition = position;
            this.IsChaoRegion = isChao;
        }

        /// <summary>
        /// Creates a new <see cref="MapRevivalRegion"/> instance from an <see cref="RgnRegion3"/> element.
        /// </summary>
        /// <param name="region"><see cref="RgnRegion3"/> instance.</param>
        /// <param name="revivalMapId">Revival map id.</param>
        /// <returns>New <see cref="MapRevivalRegion"/> instance.</returns>
        public static IMapRevivalRegion FromRgnElement(RgnRegion3 region, int revivalMapId)
            => new MapRevivalRegion(region.Left, region.Top, region.Right, region.Bottom,
                                    revivalMapId, region.Key, region.Position.Clone(), region.ChaoKey);
    }
}
