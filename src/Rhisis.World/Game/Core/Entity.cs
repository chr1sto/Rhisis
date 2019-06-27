using Rhisis.Core.Helpers;
using Rhisis.World.Game.Common;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Core.Systems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Game.Core
{
    /// <summary>
    /// Describes the Entity implementation.
    /// </summary>
    public abstract class Entity : IEntity
    {
        private bool _disposedValue;

        /// <inheritdoc />
        public uint Id { get; }

        /// <inheritdoc />
        public IContext Context { get; private set; }

        /// <inheritdoc />
        public abstract WorldEntityType Type { get; }

        /// <inheritdoc />
        public ObjectComponent Object { get; set; }

        /// <inheritdoc />
        public Delayer Delayer { get; }

        /// <summary>
        /// Creates a new <see cref="Entity"/> instance.
        /// </summary>
        /// <param name="context"></param>
        protected Entity(IContext context)
        {
            this.Id = RandomHelper.GenerateUniqueId();
            this.Context = context;
            this.Object = new ObjectComponent();
            this.Delayer = new Delayer();
        }

        /// <inheritdoc />
        public void NotifySystem<TSystem>(SystemEventArgs e = null)
            where TSystem : ISystem => SystemManager.Instance.Execute<TSystem>(this, e);

        /// <inheritdoc />
        public TEntity FindEntity<TEntity>(uint id)
            where TEntity : IEntity => (TEntity)this.Object.Entities.FirstOrDefault(x => x is TEntity && x.Id == id);

        /// <inheritdoc />
        public void Delete() => this.Context.DeleteEntity(this);

        /// <inheritdoc />
        public void SwitchContext(IContext newContext)
        {
            this.Delete();
            this.Context = newContext;
            this.Context.AddEntity(this);
        }

        /// <inheritdoc />
        public bool Equals(IEntity x, IEntity y) => x.Equals(y);

        /// <inheritdoc />
        public bool Equals(IEntity other) 
            => (this.Id, this.Type, this.Object.MapId, this.Object.LayerId) == (other.Id, other.Type, other.Object.MapId, other.Object.LayerId);

        /// <inheritdoc />
        public int GetHashCode(IEntity obj) 
            => (this.Id, this.Type, this.Object.Name, this.Object.Type).GetHashCode();

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
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    // Dispose resources
                }

                this._disposedValue = true;
            }
        }
    }
}