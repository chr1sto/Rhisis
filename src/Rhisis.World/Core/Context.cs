using Rhisis.Core.IO;
using Rhisis.World.Core.Entities;
using Rhisis.World.Core.Systems;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rhisis.World.Core
{
    public class Context : IContext, IDisposable
    {
        private static readonly object _syncSystemLock = new object();
        private static readonly Lazy<IContext> lazyInstance = new Lazy<IContext>(() => new Context());

        /// <summary>
        /// Gets a shared context instance.
        /// </summary>
        public static IContext Shared => lazyInstance.Value;

        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;
        private readonly IDictionary<Guid, IEntity> _entities;
        private readonly IList<ISystem> _systems;
        private bool _disposedValue;
        
        /// <summary>
        /// Gets the entities of the present context.
        /// </summary>
        public IReadOnlyCollection<IEntity> Entities => this._entities.Values as IReadOnlyCollection<IEntity>;

        /// <summary>
        /// Gets the systems of the present context.
        /// </summary>
        public IReadOnlyCollection<ISystem> Systems => this._systems as IReadOnlyCollection<ISystem>;

        /// <summary>
        /// Creates and initializes a new <see cref="Context"/>.
        /// </summary>
        public Context()
        {
            this._cancellationTokenSource = new CancellationTokenSource();
            this._cancellationToken = this._cancellationTokenSource.Token;
            this._entities = new ConcurrentDictionary<Guid, IEntity>();
            this._systems = new List<ISystem>();
        }

        /// <summary>
        /// Destroys and cleans the current <see cref="Context"/>.
        /// </summary>
        ~Context()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Creates a new entity in the current context.
        /// </summary>
        /// <returns>Entity</returns>
        public IEntity CreateEntity()
        {
            var entity = new Entity(this);
            
            if (this._entities.TryAdd(entity.Id, entity))
                return entity;

            return null;
        }

        /// <summary>
        /// Deletes the entity from this context.
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        /// <returns></returns>
        public bool DeleteEntity(IEntity entity)
        {
            bool removed = this._entities.Remove(entity.Id);

            return removed;
        }

        /// <summary>
        /// Find an entity by his id.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <returns>The entity</returns>
        public IEntity FindEntity(Guid id) => this._entities.TryGetValue(id, out IEntity entity) ? entity : null;

        /// <summary>
        /// Add a new system to the context.
        /// </summary>
        /// <param name="system"></param>
        public void AddSystem(ISystem system) => this._systems.Add(system);

        /// <summary>
        /// Removes a system from the context.
        /// </summary>
        /// <param name="system"></param>
        public void RemoveSystem(ISystem system) => this._systems.Remove(system);

        /// <summary>
        /// Notifies and executes a system for the entity passed as parameter aslong with the event args.
        /// </summary>
        /// <typeparam name="T">System type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="e">Arguments parameter</param>
        public void NotifySystem<T>(IEntity entity, EventArgs e) where T : IReactiveSystem
        {
            var system = this._systems.FirstOrDefault(x => x.GetType() == typeof(T)) as IReactiveSystem;

            system?.Execute(entity, e);
        }

        /// <summary>
        /// Update the systems of this context.
        /// </summary>
        /// <param name="delay"></param>
        public void StartSystemUpdate(int delay)
        {
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    if (this._cancellationToken.IsCancellationRequested)
                        break;

                    foreach (var entity in this._entities)
                    {
                        foreach (var system in this._systems)
                        {
                            if (system is IUpdateSystem updateSystem && updateSystem.Match(entity.Value))
                                updateSystem.Execute(entity.Value);
                        }
                    }

                    await Task.Delay(delay).ConfigureAwait(false);
                }
            }, this._cancellationToken);
        }

        /// <summary>
        /// Stop the system update process within this context.
        /// </summary>
        public void StopSystemUpdate()
        {
            this._cancellationTokenSource.Cancel();
        }

        /// <summary>
        /// Dispose the resources of this context.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    this.StopSystemUpdate();

                    foreach (var entity in this.Entities)
                        entity.Dispose();

                    this._entities.Clear();
                    this._systems.Clear();
                }

                this._disposedValue = true;
            }
        }
        
        /// <summary>
        /// Dispose the resources of this context.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
