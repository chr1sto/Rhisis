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
        public MapRevivalRegion(int x, int z, int width, int length, int mapId, string key, Vector3 position)
            : base(x, z, width, length)
        {
            this.MapId = mapId;
            this.Key = key;
            this.RevivalPosition = position;
        }
    }
}
