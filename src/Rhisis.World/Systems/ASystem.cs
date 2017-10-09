using Rhisis.World.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.World.Systems
{
    /// <summary>
    /// System abstraction
    /// </summary>
    public abstract class ASystem
    {
        /// <summary>
        /// Gets the system's id.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the entities attached to this system.
        /// </summary>
        protected IEnumerable<IEntity> Entities { get; private set; }

        /// <summary>
        /// Gets the filter method to apply on refresh.
        /// </summary>
        protected virtual Func<IEntity, bool> Filter { get; }

        /// <summary>
        /// Creates and initializes the <see cref="ASystem"/>.
        /// </summary>
        protected ASystem()
        {
            this.Id = Guid.NewGuid();
            this.Entities = default(IEnumerable<IEntity>);
            this.Filter = default(Func<IEntity, bool>);
        }

        /// <summary>
        /// Refresh the system's entity list.
        /// </summary>
        public void Refresh()
        {
        }

        /// <summary>
        /// Updates the system.
        /// </summary>
        public abstract void Update();
    }
}
