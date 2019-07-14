using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rhisis.Core.Handlers.Attributes;
using Rhisis.Core.Handlers.Internal;
using Rhisis.Core.Handlers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhisis.Core.Handlers
{
    public static class HandlerServiceCollectionExtensions
    {
        /// <summary>
        /// Add handlers to the container.
        /// </summary>
        /// <param name="services">Service collection container.</param>
        public static void AddHandlers(this IServiceCollection services)
        {
            services.TryAddSingleton<IHandlerActionCache>(s => HandlerCacheFactory());
            services.TryAddSingleton<HandlerActionInvokerCache>();
            services.TryAddSingleton<IHandlerInvoker, HandlerActionInvoker>();
            services.TryAddSingleton<IHandlerFactory, HandlerFactory>();

            services.TryAddSingleton<ITypeActivatorCache, TypeActivatorCache>();
        }

        /// <summary>
        /// Loads handlers and handler actions and store them into a cache.
        /// </summary>
        /// <returns><see cref="HandlerActionCache"/> containing the handler actions.</returns>
        private static HandlerActionCache HandlerCacheFactory()
        {
            var handlerCacheEntries = new Dictionary<object, HandlerActionModel>();
            IEnumerable<Type> handlers = from x in Assembly.GetEntryAssembly().GetTypes()
                                         where x.GetCustomAttributes<HandlerAttribute>().Any()
                                         select x;

            foreach (Type handlerType in handlers)
            {
                TypeInfo handlerTypeInfo = handlerType.GetTypeInfo();
                IEnumerable<HandlerActionModel> handlerActions = from x in handlerType.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                                                                 let attribute = x.GetCustomAttribute<HandlerActionAttribute>()
                                                                 where attribute != null
                                                                 select new HandlerActionModel(attribute.Action, x, handlerTypeInfo);

                foreach (HandlerActionModel handlerAction in handlerActions)
                    if (!handlerCacheEntries.ContainsKey(handlerAction.ActionType))
                        handlerCacheEntries.Add(handlerAction.ActionType, handlerAction);
            }

            return new HandlerActionCache(handlerCacheEntries);
        }
    }
}
