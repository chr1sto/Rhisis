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
        /// Creates a new default <see cref="BehaviorAttribute"/> instance.
        /// </summary>
        public BehaviorAttribute()
            : this(DefaultMoverId)
        { }

        /// <summary>
        /// Creates a new <see cref="BehaviorAttribute"/> for a specific mover id.
        /// </summary>
        /// <param name="moverId"></param>
        public BehaviorAttribute(int moverId)
        {
            this.MoverId = moverId;
        }
    }
}
