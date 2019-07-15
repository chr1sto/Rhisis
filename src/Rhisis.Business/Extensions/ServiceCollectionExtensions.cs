using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Helpers;
using System;
using System.Linq;

namespace Rhisis.Business.Extensions
{
    /// <summary>
    /// Provides extensions for the <see cref="IServiceCollection"/> object.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Rhisis business services.
        /// </summary>
        /// <param name="serviceCollection">Service collection.</param>
        public static void AddRhisisServices(this IServiceCollection serviceCollection)
        {
            var services = ReflectionHelper.GetClassesWithCustomAttribute<InjectableAttribute>();

            foreach (var serviceType in services)
            {
                var serviceAttribute = serviceType.GetCustomAttributes(false)
                    .FirstOrDefault(x => x.GetType() == typeof(InjectableAttribute)) as InjectableAttribute;
                var serviceInterfaces = serviceType.GetInterfaces();
                var serviceLifeTime = serviceAttribute != null ? serviceAttribute.LifeTime : ServiceLifetime.Transient;

                if (serviceInterfaces.Any())
                {
                    foreach (var serviceInterface in serviceInterfaces)
                    {
                        serviceCollection.Add(new ServiceDescriptor(serviceInterface, serviceType, serviceLifeTime));
                    }
                }
                else
                {
                    serviceCollection.Add(new ServiceDescriptor(serviceType, x => Activator.CreateInstance(serviceType), serviceLifeTime));
                }
            }
        }
    }
}
