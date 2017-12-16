using System;
using System.Collections.Generic;

namespace Rhisis.World.Game.Core.Interfaces
{
    /// <summary>
    /// Defines the context where the systems and entities lives.
    /// </summary>
    public interface IContext : IDisposable
    {
        /// <summary>
        /// Gets the context update time.
        /// </summary>
        double Time { get; }

        /// <summary>
        /// Gets a read-only collection of the systems of this context.
        /// </summary>
        IReadOnlyList<ISystem> Systems { get; }

        /// <summary>
        /// Gets a read-only collection of the entities of this context.
        /// </summary>
        IReadOnlyList<IEntity> Entities { get; }

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <typeparam name="TEntity">Entity concrete type.</typeparam>
        /// <returns>New entity</returns>
        TEntity CreateEntity<TEntity>() where TEntity : class, IEntity;

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        /// <returns>Deleted state</returns>
        bool DeleteEntity(IEntity entity);

        /// <summary>
        /// Finds an entity by his id.
        /// </summary>
        /// <param name="id">Entity id</param>
        /// <returns>The found entity</returns>
        IEntity FindEntity(int id);

        /// <summary>
        /// Starts the context update.
        /// </summary>
        /// <param name="delay"></param>
        void StartSystemUpdate(int delay);

        /// <summary>
        /// Stops the context update.
        /// </summary>
        void StopSystemUpdate();

        /// <summary>
        /// Adds a new system to the context.
        /// </summary>
        /// <param name="system">System</param>
        void AddSystem(ISystem system);

        /// <summary>
        /// Removes a system from the context.
        /// </summary>
        /// <param name="system"></param>
        void RemoveSystem(ISystem system);

        /// <summary>
        /// Notify a system of this context to be executed.
        /// </summary>
        /// <typeparam name="T">System type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="e">Arguments</param>
        void NotifySystem<T>(IEntity entity, EventArgs e) where T : class, INotifiableSystem;
    }
}
