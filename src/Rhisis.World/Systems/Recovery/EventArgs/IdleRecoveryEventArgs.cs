using Rhisis.World.Game.Core.Systems;

namespace Rhisis.World.Systems.Recovery.EventArgs
{
    /// <summary>
    /// Idle recovery event arguments.
    /// </summary>
    public sealed class IdleRecoveryEventArgs : SystemEventArgs
    {
        /// <summary>
        /// Gets a value that indicates if the player is sitted or not.
        /// </summary>
        public bool IsSitted { get; }

        /// <summary>
        /// Creates a new <see cref="IdleRecoveryEventArgs"/> instance.
        /// </summary>
        /// <param name="isSitted">Player sit state.</param>
        public IdleRecoveryEventArgs(bool isSitted)
        {
            this.IsSitted = isSitted;
        }

        /// <inheritdoc />
        public override bool CheckArguments() => true;
    }
}
