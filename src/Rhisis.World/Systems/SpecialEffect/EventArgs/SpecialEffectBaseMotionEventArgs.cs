using Rhisis.Core.Data;
using Rhisis.World.Game.Core.Systems;
using Rhisis.World.Game.Structures;

namespace Rhisis.World.Systems.SpecialEffect.EventArgs
{
    /// <summary>
    /// Describes a special effect base motion event arguments used for delayed item usage states.
    /// </summary>
    public sealed class SpecialEffectBaseMotionEventArgs : SystemEventArgs
    {
        /// <summary>
        /// Gets the state mode base motion.
        /// </summary>
        public StateModeBaseMotion Motion { get; }

        /// <summary>
        /// Gets the item to be used and to be activated.
        /// </summary>
        public Item Item { get; }

        /// <summary>
        /// Creates a new <see cref="SpecialEffectBaseMotionEventArgs"/> instance.
        /// </summary>
        /// <param name="motion">Motion.</param>
        /// <param name="item">Item to be used.</param>
        public SpecialEffectBaseMotionEventArgs(StateModeBaseMotion motion, Item item = null)
        {
            this.Motion = motion;
            this.Item = item;
        }

        /// <inheritdoc />
        public override bool GetCheckArguments()
        {
            return true;
        }
    }
}
