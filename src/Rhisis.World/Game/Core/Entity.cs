using Rhisis.Core.Helpers;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Rhisis.World.Game.Core
{
    /// <summary>
    /// Implementation of the basic entity class.
    /// </summary>
    public class Entity : IEntity, IDisposable, IEqualityComparer<IEntity>
    {
        private bool _disposedValue;

        /// <summary>
        /// Gets the entity unique id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the entity type.
        /// </summary>
        public virtual WorldEntityType Type { get; }

        /// <summary>
        /// Gets the entity parent context.
        /// </summary>
        public IContext Context { get; }

        /// <summary>
        /// Gets the entity object component.
        /// </summary>
        public ObjectComponent ObjectComponent { get; set; }

        /// <summary>
        /// Creates a new <see cref="Entity"/> instance.
        /// </summary>
        /// <param name="context"></param>
        internal Entity(IContext context)
        {
            this.Id = RandomHelper.GenerateUniqueId();
            this.Context = context;
            this.ObjectComponent = new ObjectComponent();
        }

        /// <summary>
        /// Check if this entity is the same has another.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(IEntity x, IEntity y) => x.Id == y.Id;

        /// <summary>
        /// Gets the hash code of this instance.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(IEntity obj)
        {
            int hashCode = obj.Id ^ (int)obj.Type;

            return hashCode.GetHashCode();
        }

        /// <summary>
        /// Dispose the <see cref="Entity"/> resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose the resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    // TODO: delete resources
                }

                this._disposedValue = true;
            }
        }
    }
}
