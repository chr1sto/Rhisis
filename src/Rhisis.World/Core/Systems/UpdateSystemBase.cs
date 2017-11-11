using System;
using Rhisis.World.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Core.Systems
{
    public abstract class UpdateSystemBase : IUpdateSystem
    {
        public virtual Func<IEntity, bool> Filter => x => true;

        protected IContext Context { get; }

        protected IEnumerable<IEntity> Entities { get; private set; }

        protected UpdateSystemBase(IContext context)
        {
            this.Context = context;
            this.Entities = new List<IEntity>();
        }

        public abstract void Execute();

        public void Refresh()
        {
            this.Entities = this.Context.Entities.Where(this.Filter);
        }
    }
}
