using System;
using Rhisis.World.Core.Entities;
using System.Collections.Generic;

namespace Rhisis.World.Core.Systems
{
    public abstract class UpdateSystemBase : IUpdateSystem
    {
        protected abstract Func<IEntity, bool> Filter { get; }

        protected IContext Context { get; }

        protected IEnumerable<IEntity> Entities => this.Context.Entities;

        protected UpdateSystemBase(IContext context)
        {
            this.Context = context;
        }

        public bool Match(IEntity entity) => this.Filter.Invoke(entity);

        public abstract void Execute(IEntity entity);
    }
}
