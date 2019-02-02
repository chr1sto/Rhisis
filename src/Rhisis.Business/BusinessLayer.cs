using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Helpers;
using System.Linq;

namespace Rhisis.Business
{
    public class BusinessLayer
    {
        /// <summary>
        /// Initializes the Business Layer and loads services into the dependency container.
        /// </summary>
        public static void Initialize()
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
                        DependencyContainer.Instance.Register(serviceInterface, serviceType, serviceLifeTime);
                    }
                }
                else
                {
                    DependencyContainer.Instance.Register(serviceType, serviceLifeTime);
                }
            }
        }
    }
}
