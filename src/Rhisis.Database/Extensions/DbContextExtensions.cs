using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Rhisis.Database.Extensions
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// Detaches the local entity from the db context and attaches the target entity to the current context.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <param name="context">Current database context.</param>
        /// <param name="entity">Current entity to attach.</param>
        public static void DetachLocal<TEntity>(this DbContext context, TEntity entity)
            where TEntity : class
        {
            var localEntity = context.Set<TEntity>().Local.FirstOrDefault(x => x.Equals(entity));

            if (localEntity != null)
            {
                context.Entry(localEntity).State = EntityState.Detached;
            }

            context.Entry(entity).State = EntityState.Modified;
        }
    }
}
