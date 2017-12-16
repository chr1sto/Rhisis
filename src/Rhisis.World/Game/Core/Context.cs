using Rhisis.Core.IO;
using Rhisis.World.Game.Core.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rhisis.World.Game.Core
{
    /// <summary>
    /// Implementation of a context.
    /// </summary>
    public class Context : IContext, IDisposable
    {
        private static readonly object _syncPlayersLock = new object();

        private bool _disposedValue;

        private readonly IList<ISystem> _systems;
        private readonly IList<IEntity> _playersEntities;
        private readonly IDictionary<int, IEntity> _entities;
        private readonly CancellationToken _cancellationToken;
        private readonly CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// Gets the context update time.
        /// </summary>
        public double Time { get; private set; }

        /// <summary>
        /// Gets a read-only collection of the systems of this context.
        /// </summary>
        public IReadOnlyList<ISystem> Systems => this._systems as IReadOnlyList<ISystem>;

        /// <summary>
        /// Gets a read-only collection of the entities of this context.
        /// </summary>
        public IReadOnlyList<IEntity> Entities => this._entities.Values as IReadOnlyList<IEntity>;

        /// <summary>
        /// Creates a new <see cref="Context"/> instance.
        /// </summary>
        public Context()
        {
            this._systems = new List<ISystem>();
            this._playersEntities = new List<IEntity>();
            this._entities = new ConcurrentDictionary<int, IEntity>();
            this._cancellationTokenSource = new CancellationTokenSource();
            this._cancellationToken = this._cancellationTokenSource.Token;
        }

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <typeparam name="TEntity">Entity concrete type.</typeparam>
        /// <returns>New entity</returns>
        public TEntity CreateEntity<TEntity>() where TEntity : class, IEntity
        {
            var entity = Activator.CreateInstance(typeof(TEntity), this) as IEntity;

            this._entities.Add(entity.Id, entity);

            if (entity.Type == WorldEntityType.Player)
            {
                lock (_syncPlayersLock)
                {
                    this._playersEntities.Add(entity);
                }
            }
            
            return (TEntity)entity;
        }

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        /// <returns>Deleted state</returns>
        public bool DeleteEntity(IEntity entity)
        {
            bool removed = this._entities.Remove(entity.Id);

            if (entity.Type == WorldEntityType.Player)
            {
                lock (_syncPlayersLock)
                {
                    removed = this._playersEntities.Remove(entity);
                }
            }

            return removed;
        }

        /// <summary>
        /// Finds an entity by his id.
        /// </summary>
        /// <param name="id">Entity id</param>
        /// <returns>The found entity</returns>
        public IEntity FindEntity(int id) => this._entities.TryGetValue(id, out IEntity value) ? value : null;

        /// <summary>
        /// Starts the context update.
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

                    try
                    {
                        lock (_syncPlayersLock)
                        {
                            foreach (var entity in this._playersEntities)
                            {
                                foreach (var system in this._systems)
                                {
                                    if (!(system is INotifiableSystem) && system.Match(entity))
                                        system.Execute(entity);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Error("Context error: {0}", e.Message);
                        Logger.Debug(e.StackTrace);
                    }

                    await Task.Delay(delay).ConfigureAwait(false);
                }
            }, this._cancellationToken);
        }

        /// <summary>
        /// Stops the context update.
        /// </summary>
        public void StopSystemUpdate()
        {
            this._cancellationTokenSource.Cancel();
        }

        /// <summary>
        /// Adds a new system to the context.
        /// </summary>
        /// <param name="system">System</param>
        public void AddSystem(ISystem system) => this._systems.Add(system);

        /// <summary>
        /// Removes a system from the context.
        /// </summary>
        /// <param name="system"></param>
        public void RemoveSystem(ISystem system) => this._systems.Remove(system);

        /// <summary>
        /// Notify a system of this context to be executed.
        /// </summary>
        /// <typeparam name="T">System type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="e">Arguments</param>
        public void NotifySystem<T>(IEntity entity, EventArgs e) where T : class, INotifiableSystem
        {
            if (this._systems.FirstOrDefault(x => x.GetType() == typeof(T)) is INotifiableSystem system && system.Match(entity))
                system.Execute(entity, e);
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
