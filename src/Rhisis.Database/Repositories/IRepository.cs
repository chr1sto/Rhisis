using Rhisis.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rhisis.Database.Repositories
{
    /// <summary>
    /// Describes the repository behavior.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : DbEntity
    {
        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Create(T entity);

        /// <summary>
        /// Creates a new entity asynchronously.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> CreateAsync(T entity);

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
        T Get(Expression<Func<T, bool>> func);
        
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
        IEnumerable<T> GetAll(Expression<Func<T, bool>> func);

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
        int Count(Expression<Func<T, bool>> func);

        /// <summary>
        /// Check if there is entities that matches the predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        bool HasAny(Expression<Func<T, bool>> predicate);
    }
}
