using Rhisis.Core.Helpers;
using Rhisis.Core.Structures;

namespace Rhisis.World.Game.Regions
{
    /// <summary>
    /// Abstract implementation of a region.
    /// </summary>
    public abstract class Region : IRegion
    {
        /// <inheritdoc />
        public int X { get; }

        /// <inheritdoc />
        public int Z { get; }

        /// <inheritdoc />
        public int Width { get; }

        /// <inheritdoc />
        public int Length { get; }

        /// <summary>
        /// Creates a new <see cref="Region"/> object.
        /// </summary>
        /// <param name="x">X coordinate (X Top Left corner)</param>
        /// <param name="z">Z coordinate (Z top left corner)</param>
        /// <param name="width">Width of the region</param>
        /// <param name="length">Length of the region</param>
        protected Region(int x, int z, int width, int length)
        {
            this.X = x;
            this.Z = z;
            this.Width = width - x;
            this.Length = length - z;
        }

        /// <inheritdoc />
        public Vector3 GetRandomPosition()
        {
            var position = new Vector3()
            {
                X = RandomHelper.FloatRandom(this.X, this.X + this.Width),
                Y = 0,
                Z = RandomHelper.FloatRandom(this.Z, this.Z + this.Length)
            };

            return position;
        }

        /// <inheritdoc />
        public bool Contains(Vector3 position) => position.X >= this.X && position.X <= this.X + this.Width &&
                                                  position.Z >= this.Z && position.Z <= this.Z + this.Length;
    }
}
