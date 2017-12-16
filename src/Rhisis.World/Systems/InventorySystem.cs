using Rhisis.World.Core.Systems;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;
using System;
using System.Linq.Expressions;

namespace Rhisis.World.Systems
{
    [System]
    public class InventorySystem : NotifiableSystemBase
    {
        protected override Expression<Func<IEntity, bool>> Filter => x => x.Type == WorldEntityType.Player;

        public InventorySystem(IContext context)
            : base(context)
        {
        }

        public override void Execute(IEntity entity, EventArgs e)
        {
        }
    }
}
