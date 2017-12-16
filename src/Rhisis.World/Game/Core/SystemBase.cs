using Rhisis.World.Game.Core.Interfaces;
using System;
using System.Linq.Expressions;

namespace Rhisis.World.Game.Core
{
    public abstract class SystemBase : ISystem
    {
        private Func<IEntity, bool> _filter;

        protected IContext Context { get; }

        protected virtual Expression<Func<IEntity, bool>> Filter { get; }

        public SystemBase(IContext context)
        {
            this.Context = context;
        }

        public bool Match(IEntity entity)
        {
            this._filter = this._filter ?? this.Filter.Compile();

            return this._filter != null ? this._filter.Invoke(entity) : false;
        }

        public virtual void Execute(IEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
