using Rhisis.Core.Structures;

namespace Rhisis.World.Game.Maps.Regions
{
    public class MapRegion : Rectangle, IMapRegion
    {
        /// <inheritdoc />
        public bool IsActive { get; set; }

        /// <summary>
        /// Creates a new <see cref="MapRegion"/> instance.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <param name="width"></param>
        /// <param name="length"></param>
        public MapRegion(int x, int z, int width, int length)
            : base(x, z, width, length)
        {
        }

        /// <inheritdoc />
        public Rectangle GetRectangle() => this;

        /// <inheritdoc />
        public virtual object Clone()
        {
            return new MapRegion(X, Z, Width, Length)
            {
                IsActive = IsActive
            };
        }
    }
}
