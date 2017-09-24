using Microsoft.EntityFrameworkCore;
using Rhisis.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Database.Repositories
{
    public abstract class ARepository<T> : IRepository<T> 
        where T : class, IDatabaseEntity
    {
        protected DbContext Context { get; private set; }

        protected ARepository(DbContext context)
        {
            this.Context = context;
        }

        public T Create(T entity)
        {
            this.Context.Set<T>().Add(entity);
            this.Context.SaveChanges();

            return entity;
        }

        public T Delete(T Entity)
        {
            throw new NotImplementedException();
        }

        public T Get(int id) => this.GetQueryable().FirstOrDefault(x => x.Id == id);

        public T Get(Func<T, bool> func) => this.GetQueryable().FirstOrDefault(func);

        public IEnumerable<T> GetAll() => this.GetQueryable().AsEnumerable();

        public IEnumerable<T> GetAll(Func<T, bool> func) => this.GetQueryable().Where(func).AsEnumerable();

        public T Update(T entity)
        {
            throw new NotImplementedException();
        }

        public int Count() => this.Context.Set<T>().Count();

        public int Count(Func<T, bool> func) => this.Context.Set<T>().Count(func);

        protected virtual IQueryable<T> GetQueryable()
        {
            return this.Context.Set<T>().AsQueryable();
        }
    }
}
