using Rhisis.Core.Helpers;
using Rhisis.World.Game.Components;
using System;
using System.Collections.Generic;

namespace Rhisis.World.Game.Core
{
    /// <summary>
    /// Describes the Entity implementation.
    /// </summary>
    public abstract class Entity : IEntity
    {
        private bool disposedValue = false;

        /// <inheritdoc />
        public int Id { get; }

        /// <inheritdoc />
        public IContext Context { get; }

        /// <inheritdoc />
        public abstract WorldEntityType Type { get; }

        /// <inheritdoc />
        public ObjectComponent Object { get; set; }

        /// <summary>
        /// Creates a new <see cref="Entity"/> instance.
        /// </summary>
        /// <param name="context"></param>
        protected Entity(IContext context)
        {
            this.Id = RandomHelper.GenerateUniqueId();
            this.Context = context;
            this.Object = new ObjectComponent();
        }

        /// <inheritdoc />
        public void NotifySystem<TSystem>(SystemEventArgs e) 
            where TSystem : INotifiableSystem => this.Context.NotifySystem<TSystem>(this, e);

        /// <inheritdoc />
        public bool Equals(IEntity x, IEntity y) => x.Id == y.Id;

        /// <inheritdoc />
        public int GetHashCode(IEntity obj)
        {
            var hashCode = 181846194;

            hashCode *= -1521134295 + EqualityComparer<IEntity>.Default.GetHashCode(this);

            return hashCode;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose inner resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Dispose resources
                }

                disposedValue = true;
            }
        }
    }
}