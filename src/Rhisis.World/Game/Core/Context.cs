using Rhisis.World.Game.Core.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Rhisis.World.Game.Core
{
    public class Context : IContext
    {
        private readonly IList<ISystem> _systems;
        private readonly IDictionary<int, IEntity> _entities;

        public IReadOnlyList<ISystem> Systems => this._systems as IReadOnlyList<ISystem>;

        public IReadOnlyList<IEntity> Entities => this._entities.Values as IReadOnlyList<IEntity>;

        public Context()
        {
            this._systems = new List<ISystem>();
            this._entities = new ConcurrentDictionary<int, IEntity>();
        }

        public TEntity CreateEntity<TEntity>() where TEntity : class, IEntity
        {
            throw new NotImplementedException();
        }

        public bool DeleteEntity(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public IEntity FindEntity(int id) => this._entities.TryGetValue(id, out IEntity value) ? value : null;
    }
}
