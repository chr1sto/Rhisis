using Microsoft.EntityFrameworkCore;
using Rhisis.Database.Entities;
using Rhisis.Database.Extensions;
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
            _context = context;
        }

        /// <inheritdoc />
        public T Create(T entity)
        {
            _context.Set<T>().Add(entity);

            return entity;
        }

        /// <inheritdoc />
        public async Task<T> CreateAsync(T entity)
        {
            var trackedEntity = await _context.Set<T>().AddAsync(entity);

            return trackedEntity.Entity;
        }

        /// <inheritdoc />
        public T Update(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _context.Attach(entity);
            }

            var trackedEntity = _context.Set<T>().Update(entity);

            return trackedEntity.Entity;
        }

        /// <inheritdoc />
        public T Delete(T entity)
        {
            var trackedEntity = _context.Set<T>().Remove(entity);

            return trackedEntity.Entity;
        }

        /// <inheritdoc />
        public T Get(int id) => Get(x => x.Id == id);

        /// <inheritdoc />
        public T Get(Expression<Func<T, bool>> func) => GetQueryable().FirstOrDefault(func);

        /// <inheritdoc />
        public IEnumerable<T> GetAll() => GetQueryable().AsEnumerable();

        /// <inheritdoc />
        public IEnumerable<T> GetAll(Expression<Func<T, bool>> func) => GetQueryable().Where(func).AsEnumerable();

        /// <inheritdoc />
        public int Count() => _context.Set<T>().AsNoTracking().Count();

        /// <inheritdoc />
        public int Count(Expression<Func<T, bool>> func) => _context.Set<T>().AsNoTracking().Count(func);

        /// <inheritdoc />
        public bool HasAny(Expression<Func<T, bool>> predicate) => _context.Set<T>().AsNoTracking().Any(predicate);

        /// <inheritdoc />
        protected virtual IQueryable<T> GetQueryable() => _context.Set<T>().AsQueryable();
    }
}
