using Rhisis.World.Game.Entities;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Rhisis.World.Game.Maps
{
    public class MapContext : IMapContext
    {
        private readonly ConcurrentDictionary<uint, IWorldEntity> _entities;

        /// <inheritdoc />
        public int Id { get; protected set; }

        /// <inheritdoc />
        public IReadOnlyDictionary<uint, IWorldEntity> Entities => _entities;

        /// <summary>
        /// Creates and initializes a new <see cref="MapContext"/> instance.
        /// </summary>
        protected MapContext()
        {
            _entities = new ConcurrentDictionary<uint, IWorldEntity>();
        }

        /// <inheritdoc />
        public virtual void AddEntity(IWorldEntity entityToAdd)
        {
            _entities.TryAdd(entityToAdd.Id, entityToAdd);
        }

        /// <inheritdoc />
        public virtual void DeleteEntity(IWorldEntity entityToDelete)
        {
            _entities.TryRemove(entityToDelete.Id, out _);
        }

        /// <inheritdoc />
        public TEntity GetEntity<TEntity>(uint id) where TEntity : IWorldEntity
        {
            return _entities.TryGetValue(id, out IWorldEntity entity) ? (TEntity)entity : default;
        }
    }
}
