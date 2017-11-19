using System;
using Rhisis.World.Core;
using Rhisis.World.Core.Entities;
using Rhisis.World.Core.Systems;

namespace Rhisis.World.Systems
{
    [System]
    public class InventorySystem : ReactiveSystemBase
    {
        public InventorySystem(IContext context)
            : base(context)
        {
        }

        public override void Execute(IEntity entity, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
