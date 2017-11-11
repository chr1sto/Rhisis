using Rhisis.World.Core.Entities;
using System;

namespace Rhisis.World.Core.Systems
{
    public abstract class ReactiveSystemBase : IReactiveSystem
    {
        /// <summary>
        /// Gets the system context.
        /// </summary>
        protected IContext Context { get; }

        /// <summary>
        /// Creates a new <see cref="ReactiveSystemBase"/> instance.
        /// </summary>
        /// <param name="context"></param>
        protected ReactiveSystemBase(IContext context)
        {
            this.Context = context;
        }

        /// <summary>
        /// Executes the reactive system logic.
        /// </summary>
        /// <param name="entity">Current entity</param>
        /// <param name="e">Arguments</param>
        public abstract void Execute(IEntity entity, EventArgs e);
    }
}
