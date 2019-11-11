using Microsoft.EntityFrameworkCore;
using Rhisis.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rhisis.Database.Repositories
{
    /// <summary>
    /// Abstract implementation of a repository.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RepositoryBase<T> : IRepository<T>
        where T : DbEntity
    {
        protected readonly DbContext _context;

        /// <summary>
        /// Initializes the repository <see cref="DbContext"/>.
        /// </summary>
        /// <param name="context"></param>
        protected RepositoryBase(DbContext context)
        {
            this._context = context;
        }

        /// <inheritdoc />
        public T Create(T entity)
        {
            this._context.Set<T>().Add(entity);

            return entity;
        }

        /// <inheritdoc />
        public async Task<T> CreateAsync(T entity)
        {
            var trackedEntity = await this._context.Set<T>().AddAsync(entity);

            return trackedEntity.Entity;
        }

        /// <inheritdoc />
        public T Update(T entity)
        {
            var trackedEntity = this._context.Set<T>().Update(entity);

            return trackedEntity.Entity;
        }

        /// <inheritdoc />
        public T Delete(T entity)
        {
            var trackedEntity = this._context.Set<T>().Remove(entity);

            return trackedEntity.Entity;
        }

        /// <inheritdoc />
        public T Get(int id) => this.Get(x => x.Id == id);

        /// <inheritdoc />
        public T Get(Expression<Func<T, bool>> func) => this.GetQueryable().FirstOrDefault(func);

        /// <inheritdoc />
        public IEnumerable<T> GetAll() => this.GetQueryable().AsEnumerable();

        /// <inheritdoc />
        public IEnumerable<T> GetAll(Expression<Func<T, bool>> func) => this.GetQueryable().Where(func).AsEnumerable();

        /// <inheritdoc />
        public int Count() => this._context.Set<T>().AsNoTracking().Count();

        /// <inheritdoc />
        public int Count(Expression<Func<T, bool>> func) => this._context.Set<T>().AsNoTracking().Count(func);

        /// <inheritdoc />
        public bool HasAny(Expression<Func<T, bool>> predicate) => this._context.Set<T>().AsNoTracking().Any(predicate);

        /// <inheritdoc />
        protected virtual IQueryable<T> GetQueryable() => this._context.Set<T>().AsQueryable();
    }
}
