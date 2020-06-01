using Rhisis.Core.Structures;

namespace Rhisis.World.Game.Maps.Regions
{
    public interface IMapTriggerRegion : IMapRegion
    {
        /// <summary>
        /// Gets the trigger destination map id.
        /// </summary>
        int DestinationMapId { get; }

        /// <summary>
        /// Gets the trigger destination map position.
        /// </summary>
        Vector3 DestinationMapPosition { get; }

        /// <summary>
        /// Gets a value that indicates if the current region is wrapzone.
        /// </summary>
        bool IsWrapzone { get; }
    }
}
