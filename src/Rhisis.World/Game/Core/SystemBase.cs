using Rhisis.World.Systems;
using System;
using System.Linq.Expressions;

namespace Rhisis.World.Game.Core
{
    /// <summary>
    /// Abstraction of the basic system.
    /// </summary>
    public abstract class SystemBase : ISystem
    {
        /// <summary>
        /// Gets the context of the system.
        /// </summary>
        protected IContext Context { get; }

        /// <summary>
        /// Gets the entity types that are allowed to execute the current system.
        /// </summary>
        protected virtual WorldEntityType Type { get; }

        /// <summary>
        /// Creates a new <see cref="SystemBase"/> instance.
        /// </summary>
        /// <param name="context">Context</param>
        protected SystemBase(IContext context)
        {
            this.Context = context;
        }

        /// <summary>
        /// Executes the system logic for the entity passed as parameter.
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Execute(IEntity entity) => throw new NotImplementedException();

        /// <summary>
        /// Check if the entity matches the system filter predicate.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns></returns>
        public bool Match(IEntity entity) => (entity.Type & this.Type) == entity.Type;
    }
}
