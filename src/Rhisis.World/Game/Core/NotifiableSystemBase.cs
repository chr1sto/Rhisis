using Rhisis.World.Game.Core.Interfaces;
using System;

namespace Rhisis.World.Game.Core
{
    public abstract class NotifiableSystemBase : SystemBase, INotifiableSystem
    {
        public NotifiableSystemBase(IContext context)
            : base(context)
        {
        }

        public virtual void Execute(IEntity entity, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
