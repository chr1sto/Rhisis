using System;

namespace Rhisis.World.Game.Core.Interfaces
{
    /// <summary>
    /// Describes the behavior of a Notifiable system.
    /// </summary>
    /// <remarks>
    /// This systems can be notified by notifying them from a context.
    /// </remarks>
    public interface INotifiableSystem : ISystem
    {
        /// <summary>
        /// Executes the system logic for the entity passed as parameter.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="e">Arguments</param>
        void Execute(IEntity entity, SystemEventArgs e);
    }
}
