using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.World.Systems
{
    [System]
    public sealed class BehaviorSystem : SystemBase
    {
        /// <summary>
        /// Creates a new <see cref="BehaviorSystem"/> instance.
        /// </summary>
        /// <param name="context"></param>
        public BehaviorSystem(IContext context) 
            : base(context)
        {
        }

        /// <inheritdoc />
        public override void Execute(IEntity entity)
        {
            base.Execute(entity);
        }
    }
}
