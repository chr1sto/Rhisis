using Rhisis.Core.Structures;

namespace Rhisis.World.Game.Maps.Regions
{
    public interface IMapRegion
    {
        /// <summary>
        /// Gets the top left corner X coordinate of the region.
        /// </summary>
        int X { get; }

        /// <summary>
        /// Gets the top left corner Z coordinate of the region.
        /// </summary>
        int Z { get; }

        /// <summary>
        /// Gets the region's width.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets the region's length.
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Gets or sets the region's activity state.
        /// </summary>
        bool IsActive { get; set; }

        /// <summary>
        /// Generates a random position within the region.
        /// </summary>
        /// <returns></returns>
        Vector3 GetRandomPosition();

        /// <summary>
        /// Check if the position passed as parameter is inside the region.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        bool Contains(Vector3 position);

        /// <summary>
        /// Gets the region rectangle.
        /// </summary>
        /// <returns></returns>
        Rectangle GetRectangle();

        /// <summary>
        /// Clones the current region.
        /// </summary>
        /// <returns></returns>
        object Clone();
    }
}
