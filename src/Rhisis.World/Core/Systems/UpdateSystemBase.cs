using System;
using Rhisis.World.Core.Entities;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Rhisis.World.Core.Systems
{
    public abstract class UpdateSystemBase : IUpdateSystem
    {
        private Func<IEntity, bool> _filter;

        protected abstract Expression<Func<IEntity, bool>> Filter { get; }

        protected IContext Context { get; }

        protected IEnumerable<IEntity> Entities => this.Context.Entities;

        protected UpdateSystemBase(IContext context)
        {
            this.Context = context;
        }

        public bool Match(IEntity entity)
        {
            if (this._filter == null)
                this._filter = this.Filter.Compile();

            return this._filter(entity);
        }

        public abstract void Execute(IEntity entity);
    }
}
