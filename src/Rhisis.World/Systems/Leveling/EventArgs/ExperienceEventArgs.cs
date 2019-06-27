using Rhisis.World.Game.Core.Systems;

namespace Rhisis.World.Systems.Leveling.EventArgs
{
    /// <summary>
    /// Experience event containing the amount of experience to give to an entity.
    /// </summary>
    public class ExperienceEventArgs : SystemEventArgs
    {
        /// <summary>
        /// Gets the amount of experience to give.
        /// </summary>
        public long Experience { get; }

        /// <summary>
        /// Creates a new <see cref="ExperienceEventArgs"/> instance.
        /// </summary>
        /// <param name="experience">Amount of experience to give.</param>
        public ExperienceEventArgs(long experience)
        {
            this.Experience = experience;
        }

        /// <inheritdoc />
        public override bool GetCheckArguments()
        {
            return this.Experience > 0;
        }
    }
}
