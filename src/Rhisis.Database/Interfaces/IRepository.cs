using System;
using System.Collections.Generic;

namespace Rhisis.Database.Interfaces
{
    /// <summary>
    /// Describes the repository behavior.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class, IDatabaseEntity
    {
        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Create(T entity);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Update(T entity);

        /// <summary>
        /// Delete an existing entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Delete(T entity);

        /// <summary>
        /// Gets an entity by his Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Get(int id);

        /// <summary>
        /// Gets an entity with a filter expression.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        T Get(Func<T, bool> func);
        
        /// <summary>
        /// Get all records from the repository.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Gets all records from the repository with a filter expression.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        IEnumerable<T> GetAll(Func<T, bool> func);

        /// <summary>
        /// Get the total amount of records from the repository.
        /// </summary>
        /// <returns></returns>
        int Count();

        /// <summary>
        /// Get the total amount of records from the repository with a filter expression.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        int Count(Func<T, bool> func);
    }
}
