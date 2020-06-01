using Rhisis.Core.Helpers;

namespace Rhisis.Core.Structures
{
    /// <summary>
    /// This class describes the behavior of a Rectangle in a 3D environment.
    /// </summary>
    /// <remarks>
    /// This Rectangle implementation doesn't have a Y property because in a 3D world, Y coordinate represents the heights.
    /// </remarks>
    public class Rectangle
    {
        /// <summary>
        /// Gets the X position of the rectangle.
        /// </summary>
        public int X { get; protected set; }

        /// <summary>
        /// Gets the Z position of the rectangle.
        /// </summary>
        public int Z { get; protected set; }

        /// <summary>
        /// Gets the width of the rectangle.
        /// </summary>
        public int Width { get; protected set; }

        /// <summary>
        /// Gets the length of the rectangle.
        /// </summary>
        public int Length { get; protected set; }

        /// <summary>
        /// Creates a new <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="x">X coordinate (X Top Left corner)</param>
        /// <param name="z">Z coordinate (Z top left corner)</param>
        /// <param name="width">Width of the region</param>
        /// <param name="length">Length of the region</param>
        public Rectangle(int x, int z, int width, int length)
        {
            X = x;
            Z = z;
            Width = width;
            Length = length;
        }

        /// <summary>
        /// Generates a random position inside the rectangle.
        /// </summary>
        /// <returns>A new <see cref="Vector3"/></returns>
        public Vector3 GetRandomPosition()
        {
            var position = new Vector3()
            {
                X = RandomHelper.FloatRandom(X, X + Width),
                Y = 0,
                Z = RandomHelper.FloatRandom(Z, Z + Length)
            };

            return position;
        }

        /// <summary>
        /// Check if the position passed as parameter is inside the rectangle.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool Contains(Vector3 position) => position.X >= X && position.X <= X + Width &&
                                                  position.Z >= Z && position.Z <= Z + Length;
    }
}
