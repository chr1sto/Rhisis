using Rhisis.Core.Resources;
using Rhisis.Core.Structures;

namespace Rhisis.World.Game.Maps.Regions
{
    /// <summary>
    /// Defines the structure if a trigger region.
    /// </summary>
    public sealed class MapTriggerRegion : MapRegion, IMapTriggerRegion
    {
        /// <inheritdoc />
        public int DestinationMapId { get; }

        /// <inheritdoc />
        public Vector3 DestinationMapPosition { get; }

        /// <inheritdoc />
        public bool IsWrapzone => DestinationMapId > 0;

        /// <summary>
        /// Creates a new <see cref="MapTriggerRegion"/> instance.
        /// </summary>
        /// <param name="x">Region X position.</param>
        /// <param name="z">Region Z position.</param>
        /// <param name="width">Region width.</param>
        /// <param name="length">Region length.</param>
        /// <param name="destinationMapId">Region trigger destination map id.</param>
        /// <param name="destinationMapPosition">Region trigger destination map position.</param>
        public MapTriggerRegion(int x, int z, int width, int length, int destinationMapId, Vector3 destinationMapPosition) 
            : base(x, z, width, length)
        {
            DestinationMapId = destinationMapId;
            DestinationMapPosition = destinationMapPosition;
        }

        /// <summary>
        /// Creates a new <see cref="MapTriggerRegion"/> instance from a <see cref="RgnRegion3"/> element.
        /// </summary>
        /// <param name="region"><see cref="RgnRegion3"/> element.</param>
        /// <returns>New <see cref="MapTriggerRegion"/> instance.</returns>
        public static IMapTriggerRegion FromRgnElement(RgnRegion3 region)
            => new MapTriggerRegion(region.Left, region.Top, region.Right - region.Left, region.Bottom - region.Top,
                region.TeleportWorldId, region.TeleportPosition);
    }
}
