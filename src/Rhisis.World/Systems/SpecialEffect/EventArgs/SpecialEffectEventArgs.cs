using Rhisis.Core.Data;
using Rhisis.World.Game.Core.Systems;

namespace Rhisis.World.Systems.SpecialEffect.EventArgs
{
    /// <summary>
    /// Defines an event to start a special event a the player.
    /// </summary>
    public sealed class SpecialEffectEventArgs : SystemEventArgs
    {
        /// <summary>
        /// Gets the special effect id.
        /// </summary>
        public int SpecialEffectId { get; }

        /// <summary>
        /// Gets the special effect value.
        /// </summary>
        public DefineSpecialEffects SpecialEffect { get; }

        /// <summary>
        /// Creates a new <see cref="SpecialEffectEventArgs"/> instance.
        /// </summary>
        /// <param name="specialEffectId">Special Effect id.</param>
        public SpecialEffectEventArgs(int specialEffectId)
        {
            this.SpecialEffectId = specialEffectId;
            this.SpecialEffect = (DefineSpecialEffects)specialEffectId;
        }

        /// <summary>
        /// Creates a new <see cref="SpecialEffectEventArgs"/> instance.
        /// </summary>
        /// <param name="specialEffect">Special effect value.</param>
        public SpecialEffectEventArgs(DefineSpecialEffects specialEffect)
            : this((int)specialEffect)
        {
        }

        /// <inheritdoc />
        public override bool GetCheckArguments()
        {
            return this.SpecialEffectId > 0;
        }
    }
}
