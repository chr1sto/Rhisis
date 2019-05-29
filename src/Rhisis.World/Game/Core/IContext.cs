using System;
using System.Collections.Generic;

namespace Rhisis.World.Game.Core
{
    /// <summary>
    /// Defines a context.
    /// </summary>
    public interface IContext : IDisposable
    {
        /// <summary>
        /// Gets the list of all entities of this context.
        /// </summary>
        IEnumerable<IEntity> Entities { get; }

        /// <summary>
        /// Creates a new entity in this context.
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns></returns>
        TEntity CreateEntity<TEntity>() where TEntity : IEntity;

        /// <summary>
        /// Adds an existing entity to the current context.
        /// </summary>
        /// <param name="entity">Entity to add.</param>
        void AddEntity(IEntity entity);

        /// <summary>
        /// Find an entity in this context.
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="id">Entity id</param>
        /// <returns></returns>
        TEntity FindEntity<TEntity>(uint id) where TEntity : IEntity;

        /// <summary>
        /// Deletes the entity from this context.
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="id">Entity id</param>
        void DeleteEntity(uint id);

        /// <summary>
        /// Deletes the entity from this context.
        /// </summary>
        /// <param name="entity">Entity instance</param>
        void DeleteEntity(IEntity entity);

        /// <summary>
        /// Update this context.
        /// </summary>
        void Update();

        /// <summary>
        /// Clean up the current context. Removes the entities that needs to be deleted.
        /// </summary>
        void UpdateDeletedEntities();
    }
}