using Rhisis.Core.Common;
using Rhisis.Core.Helpers;
using Rhisis.World.Game.Common;
using Rhisis.World.Game.Components;
using System;
using System.Linq;

namespace Rhisis.World.Game.Entities.Internal
{
    public abstract class WorldEntity : IWorldEntity
    {
        private bool _disposedValue;

        /// <inheritdoc />
        public uint Id { get; set; }

        /// <inheritdoc />
        public abstract WorldEntityType Type { get; }

        /// <inheritdoc />
        public ObjectComponent Object { get; set; }

        /// <inheritdoc />
        public Delayer Delayer { get; }

        /// <summary>
        /// Creates a new <see cref="WorldEntity"/> instance.
        /// </summary>
        protected WorldEntity()
        {
            Id = RandomHelper.GenerateUniqueId();
            Object = new ObjectComponent();
            Delayer = new Delayer();
        }

        /// <inheritdoc />
        public TEntity FindEntity<TEntity>(uint id) where TEntity : IWorldEntity
            => (TEntity)Object.Entities.FirstOrDefault(x => x is TEntity && x.Id == id);

        /// <inheritdoc />
        public TEntity FindEntity<TEntity>(Func<TEntity, bool> predicate) where TEntity : IWorldEntity
            => (TEntity)Object.Entities.FirstOrDefault(x => x is TEntity entity && predicate(entity));

        /// <inheritdoc />
        public void Delete()
        {
            Object.CurrentMap.DeleteEntity(this);
            Object.CurrentLayer.DeleteEntity(this);
        }

        /// <inheritdoc />
        public bool Equals(IWorldEntity x, IWorldEntity y) => x.Equals(y);

        /// <inheritdoc />
        public bool Equals(IWorldEntity other) => Id == other.Id;

        /// <inheritdoc />
        public int GetHashCode(IWorldEntity obj) => obj.Id.GetHashCode();

        /// <summary>
        /// Disposes the <see cref="WorldEntity"/> resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Delayer.Dispose();
                }

                _disposedValue = true;
            }
        }

        /// <summary>
        /// Dispose resources.
        /// </summary>
        public void Dispose() => Dispose(true);
    }
}
