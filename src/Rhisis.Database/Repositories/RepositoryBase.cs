using Microsoft.EntityFrameworkCore;
using Rhisis.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhisis.Database.Repositories
{
    /// <summary>
    /// Abstract implementation of a repository.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RepositoryBase<T> : IRepository<T> 
        where T : class, IDatabaseEntity
    {
        private readonly DbContext _context;

        /// <summary>
        /// Creates a new <see cref="RepositoryBase{T}"/> instance.
        /// </summary>
        protected RepositoryBase()
            : this(null)
        {
        }

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
            if (this._context == null)
            {
                using (var context = DatabaseFactory.Instance.CreateDbContext())
                {
                    context.Set<T>().Add(entity);
                    context.SaveChanges();
                }
            }
            else
            {
                this._context.Set<T>().Add(entity);
                this._context.SaveChanges();
            }

            return entity;
        }

        /// <inheritdoc />
        public async Task<T> CreateAsync(T entity)
        {
            if (this._context == null)
            {
                using (var context = DatabaseFactory.Instance.CreateDbContext())
                {
                    await context.Set<T>().AddAsync(entity);
                    await context.SaveChangesAsync();
                }
            }
            else
            {
                await this._context.Set<T>().AddAsync(entity);
                await this._context.SaveChangesAsync();
            }

            return entity;
        }

        /// <inheritdoc />
        public T Update(T entity)
        {
            if (this._context == null)
            {
                using (var context = DatabaseFactory.Instance.CreateDbContext())
                {
                    context.Set<T>().Update(entity);
                    context.SaveChanges();
                }
            }
            else
            {
                this._context.Set<T>().Update(entity);
                this._context.SaveChanges();
            }

            return entity;
        }

        /// <inheritdoc />
        public async Task<T> UpdateAsync(T entity)
        {
            if (this._context == null)
            {
                using (var context = DatabaseFactory.Instance.CreateDbContext())
                {
                    context.Set<T>().Update(entity);
                    await context.SaveChangesAsync();
                }
            }
            else
            {
                this._context.Set<T>().Update(entity);
                await this._context.SaveChangesAsync();
            }

            return entity;
        }

        /// <inheritdoc />
        public T Delete(T entity)
        {
            if (this._context == null)
            {
                using (var context = DatabaseFactory.Instance.CreateDbContext())
                {
                    context.Set<T>().Remove(entity);
                    context.SaveChanges();
                }
            }
            else
            {
                this._context.Set<T>().Remove(entity);
                this._context.SaveChanges();
            }

            return entity;
        }

        /// <inheritdoc />
        public async Task<T> DeleteAsync(T entity)
        {
            if (this._context == null)
            {
                using (var context = DatabaseFactory.Instance.CreateDbContext())
                {
                    context.Set<T>().Remove(entity);
                    await context.SaveChangesAsync();
                }
            }
            else
            {
                this._context.Set<T>().Remove(entity);
                await this._context.SaveChangesAsync();
            }

            return entity;
        }

        /// <inheritdoc />
        public T Get(int id) => this.Get(x => x.Id == id);

        /// <inheritdoc />
        public T Get(Func<T, bool> func)
        {
            if (this._context == null)
            {
                using (var context = DatabaseFactory.Instance.CreateDbContext())
                {
                    return this.GetQueryable(context).FirstOrDefault(func);
                }
            }
            else
            {
                return this.GetQueryable(this._context).FirstOrDefault(func);
            }
        }

        /// <inheritdoc />
        public IEnumerable<T> GetAll()
        {
            if (this._context == null)
            {
                using (var context = DatabaseFactory.Instance.CreateDbContext())
                {
                    return this.GetQueryable(context).AsEnumerable();
                }
            }
            else
            {
                return this.GetQueryable(this._context).AsEnumerable();
            }
        }

        /// <inheritdoc />
        public IEnumerable<T> GetAll(Func<T, bool> func)
        {
            if (this._context == null)
            {
                using (var context = DatabaseFactory.Instance.CreateDbContext())
                {
                    return this.GetQueryable(context).Where(func).AsEnumerable();
                }
            }
            else
            {
                return this.GetQueryable(this._context).Where(func).AsEnumerable();
            }
        }

        /// <inheritdoc />
        public int Count()
        {
            if (this._context == null)
            {
                using (var context = DatabaseFactory.Instance.CreateDbContext())
                {
                    return this.GetQueryable(context).Count();
                }
            }
            else
            {
                return this.GetQueryable(this._context).Count();
            }
        }

        /// <inheritdoc />
        public int Count(Func<T, bool> func)
        {
            if (this._context == null)
            {
                using (var context = DatabaseFactory.Instance.CreateDbContext())
                {
                    return this.GetQueryable(context).Count(func);
                }
            }
            else
            {
                return this.GetQueryable(this._context).Count(func);
            }
        }

        /// <inheritdoc />
        protected virtual IQueryable<T> GetQueryable(DbContext context) => context.Set<T>().AsQueryable();
    }
}
