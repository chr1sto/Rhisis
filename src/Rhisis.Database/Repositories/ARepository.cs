using Microsoft.EntityFrameworkCore;
using Rhisis.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Database.Repositories
{
    /// <summary>
    /// Abstract implementation of a repository.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ARepository<T> : IRepository<T> 
        where T : class, IDatabaseEntity
    {
        private readonly DbContext _context;

        /// <summary>
        /// Initializes the repository <see cref="DbContext"/>.
        /// </summary>
        /// <param name="context"></param>
        protected ARepository(DbContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Create(T entity)
        {
            this._context.Set<T>().Add(entity);
            this._context.SaveChanges();

            return entity;
        }

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Update(T entity)
        {
            this._context.Set<T>().Update(entity);
            this._context.SaveChanges();

            return entity;
        }

        /// <summary>
        /// Delete an existing entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Delete(T entity)
        {
            this._context.Set<T>().Remove(entity);
            this._context.SaveChanges();

            return entity;
        }

        /// <summary>
        /// Gets an entity by his Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get(int id) => this.GetQueryable().FirstOrDefault(x => x.Id == id);

        /// <summary>
        /// Gets an entity with a filter expression.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public T Get(Func<T, bool> func) => this.GetQueryable().FirstOrDefault(func);

        /// <summary>
        /// Get all records from the repository.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAll() => this.GetQueryable().AsEnumerable();

        /// <summary>
        /// Gets all records from the repository with a filter expression.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAll(Func<T, bool> func) => this.GetQueryable().Where(func).AsEnumerable();

        /// <summary>
        /// Get the total amount of records from the repository.
        /// </summary>
        /// <returns></returns>
        public int Count() => this._context.Set<T>().Count();

        /// <summary>
        /// Get the total amount of records from the repository with a filter expression.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public int Count(Func<T, bool> func) => this._context.Set<T>().Count(func);

        /// <summary>
        /// Get the queryable request.
        /// </summary>
        /// <returns></returns>
        protected virtual IQueryable<T> GetQueryable()
        {
            return this._context.Set<T>().AsQueryable();
        }
    }
}
