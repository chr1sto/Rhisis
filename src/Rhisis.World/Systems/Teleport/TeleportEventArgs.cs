using Rhisis.World.Game.Core.Systems;

namespace Rhisis.World.Systems.Teleport
{
    public class TeleportEventArgs : SystemEventArgs
    {
        /// <summary>
        /// Gets the target map id.
        /// </summary>
        public int MapId { get; }

        /// <summary>
        /// Gets the target X position.
        /// </summary>
        public float PositionX { get; }

        /// <summary>
        /// Gets target Z position.
        /// </summary>
        public float PositionZ { get; }

        /// <summary>
        /// Creates a new <see cref="TeleportEventArgs"/> instance.
        /// </summary>
        /// <param name="mapId">Target Map Id.</param>
        /// <param name="positionX">Target X position.</param>
        /// <param name="positionZ">Target Z position.</param>
        public TeleportEventArgs(int mapId, float positionX, float positionZ)
        {
            this.MapId = mapId;
            this.PositionX = positionX;
            this.PositionZ = positionZ;
        }

        /// <inheritdoc />
        public override bool CheckArguments()
        {
            return this.MapId > 0 && this.PositionX > 0f && this.PositionZ > 0;
        }
    }
}
