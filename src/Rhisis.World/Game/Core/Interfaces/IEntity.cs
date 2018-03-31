using Rhisis.World.Game.Components;
using System;
using System.Collections.Generic;

namespace Rhisis.World.Game.Core.Interfaces
{
    /// <summary>
    /// Defines a basic entity.
    /// </summary>
    public interface IEntity : IDisposable, IEqualityComparer<IEntity>
    {
        /// <summary>
        /// Gets the entity unique id.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Gets the entity type.
        /// </summary>
        WorldEntityType Type { get; }

        /// <summary>
        /// Gets the entity parent context.
        /// </summary>
        IContext Context { get; }

        /// <summary>
        /// Gets the entity object component.
        /// </summary>
        ObjectComponent Object { get; set; }
    }
}
