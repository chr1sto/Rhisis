using Rhisis.World.Core.Entities;
using Rhisis.World.Core.Systems;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.World.Core
{
    public class Context : IContext
    {
        private static readonly Lazy<Context> lazyInstance = new Lazy<Context>(() => new Context());

        public static Context Instance => lazyInstance.Value;

        public ICollection<IEntity> Entities => throw new NotImplementedException();

        private readonly ConcurrentDictionary<Guid, IEntity> _entities;
        private readonly List<ISystem> _systems;

        public Context()
        {
            this._entities = new ConcurrentDictionary<Guid, IEntity>();
        }

        public IEntity CreateEntity()
        {
            var entity = new Entity();

            if (this._entities.TryAdd(entity.Id, entity))
                return entity;

            return null;
        }

        public bool DeleteEntity(IEntity entity)
        {
            return this._entities.TryRemove(entity.Id, out IEntity value);
        }

        public IEntity FindEntity(Guid id)
        {
            throw new NotImplementedException();
        }

        public void AddSystem(ISystem system) => this._systems.Add(system);

        public void RemoveSystem(ISystem system) => this._systems.Remove(system);
    }
}
