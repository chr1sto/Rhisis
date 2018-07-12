using System;
using System.Linq.Expressions;

namespace Rhisis.World.Game.Core
{
    /// <summary>
    /// Abstraction of the basic system.
    /// </summary>
    public abstract class SystemBase : ISystem
    {
        private Func<IEntity, bool> _filter;

        /// <summary>
        /// Gets the context of the system.
        /// </summary>
        protected IContext Context { get; }

        /// <summary>
        /// Gets filter of the system.
        /// </summary>
        /// <remarks>
        /// This filter is used to check if the entities needs to be updated by this system.
        /// </remarks>
        protected virtual Expression<Func<IEntity, bool>> Filter { get; }

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
        public virtual void Execute(IEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check if the entity matches the system filter predicate.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns></returns>
        public bool Match(IEntity entity)
        {
            this._filter = this._filter ?? this.Filter.Compile();

            return this._filter != null ? this._filter.Invoke(entity) : false;
        }
    }
}
