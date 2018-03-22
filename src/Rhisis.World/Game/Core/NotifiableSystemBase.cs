using Rhisis.World.Game.Core.Interfaces;
using System;

namespace Rhisis.World.Game.Core
{
    /// <summary>
    /// <see cref="NotifiableSystemBase"/> implementation.
    /// </summary>
    public abstract class NotifiableSystemBase : SystemBase, INotifiableSystem
    {
        /// <summary>
        /// Creates a new <see cref="NotifiableSystemBase"/> instance.
        /// </summary>
        /// <param name="context"></param>
        protected NotifiableSystemBase(IContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Executes the system logic for the entity passed as parameter.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="e">Arguments</param>
        public virtual void Execute(IEntity entity, SystemEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
