using System;

namespace Rhisis.World.Game.Behaviors
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class BehaviorAttribute : Attribute
    {
        public const int DefaultMoverId = 0;

        /// <summary>
        /// Gets the mover's id.
        /// </summary>
        public int MoverId { get; }

        /// <summary>
        /// Get the behavior type.
        /// </summary>
        public BehaviorType Type { get; }

        /// <summary>
        /// Gets the default state of the behavior.
        /// </summary>
        public bool IsDefault { get; }

        /// <summary>
        /// Creates a new default <see cref="BehaviorAttribute"/> instance.
        /// </summary>
        /// <param name="behaviorType">Behavior type</param>
        public BehaviorAttribute(BehaviorType behaviorType, bool isDefault = false)
            : this(behaviorType, DefaultMoverId, isDefault)
        { }

        /// <summary>
        /// Creates a new <see cref="BehaviorAttribute"/> for a specific mover id.
        /// </summary>
        /// <param name="behaviorType">Behavior type</param>
        /// <param name="moverId"></param>
        /// <param name="isDefault">Indicates that this is a default behavior.</param>
        public BehaviorAttribute(BehaviorType behaviorType, int moverId, bool isDefault = false)
        {
            Type = behaviorType;
            MoverId = moverId;
            IsDefault = isDefault;
        }
    }
}
