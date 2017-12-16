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
        /// <summary>
        /// Gets the <see cref="InventorySystem"/> match filter.
        /// </summary>
        protected override Expression<Func<IEntity, bool>> Filter => x => x.Type == WorldEntityType.Player;

        /// <summary>
        /// Creates a new <see cref="InventorySystem"/> instance.
        /// </summary>
        /// <param name="context"></param>
        public InventorySystem(IContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Executes the <see cref="InventorySystem"/> logic.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        public override void Execute(IEntity entity, EventArgs e)
        {
        }
    }
}
