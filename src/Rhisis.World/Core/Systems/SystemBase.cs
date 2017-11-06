using System;
using Rhisis.World.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Core.Systems
{
    public abstract class SystemBase : ISystem
    {
        public virtual Func<IEntity, bool> Filter => x => true;

        protected IContext Context { get; }

        protected IEnumerable<IEntity> Entities { get; private set; }

        protected SystemBase(IContext context)
        {
            this.Context = context;
        }

        public abstract void Execute();

        public void Refresh()
        {
            this.Entities = this.Context.Entities.Where(this.Filter);
        }
    }
}
