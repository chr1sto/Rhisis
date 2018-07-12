using Rhisis.World.Game.Components;
using System;
using System.Collections.Generic;

namespace Rhisis.World.Game.Core
{
    /// <summary>
    /// Describes the Entity behavior.
    /// </summary>
    public interface IEntity : IDisposable, IEqualityComparer<IEntity>
    {
        /// <summary>
        /// Gets the entity id.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Gets the entity context.
        /// </summary>
        IContext Context { get; }

        /// <summary>
        /// Gets the entity type.
        /// </summary>
        WorldEntityType Type { get; }

        /// <summary>
        /// Gets the object component of this entity.
        /// </summary>
        ObjectComponent Object { get; set; }

        /// <summary>
        /// Notifies and executes a system.
        /// </summary>
        /// <typeparam name="TSystem">System type</typeparam>
        /// <param name="e">System event arguments</param>
        void NotifySystem<TSystem>(SystemEventArgs e) where TSystem : INotifiableSystem;
    }
}