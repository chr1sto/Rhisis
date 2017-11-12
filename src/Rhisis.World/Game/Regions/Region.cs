using Rhisis.Core.Helpers;
using Rhisis.Core.Structures;

namespace Rhisis.World.Game.Regions
{
    /// <summary>
    /// Abstract implementation of a region.
    /// </summary>
    public abstract class Region : IRegion
    {
        /// <summary>
        /// Gets the top left corner X coordinate of the region.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Gets the top left corner Z coordinate of the region.
        /// </summary>
        public int Z { get; }

        /// <summary>
        /// Gets the region's width.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Gets the region's length.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Creates a new <see cref="Region"/> object.
        /// </summary>
        /// <param name="left">X coordinate (X Top Left corner)</param>
        /// <param name="top">Z coordinate (Z top left corner)</param>
        /// <param name="right">Width of the region</param>
        /// <param name="bottom">Height of the region</param>
        public Region(int left, int top, int right, int bottom)
        {
            this.X = left;
            this.Z = top;
            this.Width = right - left;
            this.Length = bottom - top;
        }

        /// <summary>
        /// Generate a random position within the region.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Check if the position passed as parameter is inside the region.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>True when the position is inside the region, otherwise false</returns>
        public bool Contains(Vector3 position)
        {
            return position.X >= this.X && position.X <= this.X + this.Width &&
                position.Z >= this.Z && position.Z <= this.Z + this.Length;
        }
    }
}
