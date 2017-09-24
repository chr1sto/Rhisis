using System;
using System.Collections;
using System.Collections.Generic;

namespace Rhisis.Database.Interfaces
{
    /// <summary>
    /// Describes the repository behavior.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class, IDatabaseEntity
    {
        IEnumerable<T> GetAll();

        IEnumerable<T> GetAll(Func<T, bool> func);

        T Get(int id);

        T Get(Func<T, bool> func);

        T Create(T entity);

        T Update(T entity);

        T Delete(T entity);

        int Count();

        int Count(Func<T, bool> func);
    }
}
