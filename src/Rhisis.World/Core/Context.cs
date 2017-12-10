using Rhisis.Core.IO;
using Rhisis.World.Core.Entities;
using Rhisis.World.Core.Systems;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rhisis.World.Core
{
    public class Context : IContext, IDisposable
    {
        private static readonly object _syncSystemLock = new object();
        private static readonly object _syncPlayersLock = new object();
        private static readonly Lazy<IContext> lazyInstance = new Lazy<IContext>(() => new Context());

        /// <summary>
        /// Gets a shared context instance.
        /// </summary>
        public static IContext Shared => lazyInstance.Value;

        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;
        private readonly IDictionary<Guid, IEntity> _entities;
        private readonly IList<IEntity> _playersEntities;
        private readonly IList<ISystem> _systems;
        private bool _disposedValue;

        public double Time { get; private set; }

        /// <summary>
        /// Gets the entities of the present context.
        /// </summary>
        public ICollection<IEntity> Entities => this._entities.Values;

        /// <summary>
        /// Gets the systems of the present context.
        /// </summary>
        public ICollection<ISystem> Systems => this._systems;

        /// <summary>
        /// Creates and initializes a new <see cref="Context"/>.
        /// </summary>
        public Context()
        {
            this._cancellationTokenSource = new CancellationTokenSource();
            this._cancellationToken = this._cancellationTokenSource.Token;
            this._entities = new ConcurrentDictionary<Guid, IEntity>();
            this._playersEntities = new List<IEntity>();
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
        /// Creates a new entity in the current context and set his type.
        /// </summary>
        /// <param name="type">Entity type</param>
        /// <returns></returns>
        public IEntity CreateEntity(WorldEntityType type)
        {
            var entity = this.CreateEntity();

            entity.EntityType = type;

            if (entity.EntityType == WorldEntityType.Player)
            {
                lock (_syncPlayersLock)
                {
                    this._playersEntities.Add(entity);
                }
            }

            return entity;
        }

        /// <summary>
        /// Deletes the entity from this context.
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        /// <returns></returns>
        public bool DeleteEntity(IEntity entity)
        {
            bool removed = this._entities.Remove(entity.Id);

            if (entity.EntityType == WorldEntityType.Player && this._playersEntities.Contains(entity))
            {
                lock (_syncPlayersLock)
                {
                    removed = this._playersEntities.Remove(entity);
                }
            }

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
            Task.Run(async () =>
            {
                double deltaTime = 0f;
                double currentTime = 0f;
                double previousTime = 0f;

                while (true)
                {
                    if (this._cancellationToken.IsCancellationRequested)
                        break;

                    currentTime = Rhisis.Core.IO.Time.TimeInMilliseconds();
                    deltaTime = currentTime - previousTime;
                    previousTime = currentTime;

                    this.Time = deltaTime / 1000f;

                    lock (_syncPlayersLock)
                    {
                        foreach (var entity in this._playersEntities)
                        {
                            for (int i = 0; i < this._systems.Count; i++)
                            {
                                if (this._systems[i] is IUpdateSystem updateSystem)
                                    updateSystem.Execute(entity);
                            }
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
