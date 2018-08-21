using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.Common;
using Rhisis.Core.Reflection;
using System;
using System.Linq;

namespace Rhisis.Core.DependencyInjection
{
    public class DependencyContainer : Singleton<DependencyContainer>
    {
        private IServiceCollection _services;
        private IServiceProvider _serviceProvider;
        private bool _isInitialized;

        /// <summary>
        /// Creates a new <see cref="DependencyContainer"/> instance.
        /// </summary>
        public DependencyContainer()
        {
            this._services = new ServiceCollection();
        }

        /// <summary>
        /// Initialize the dependency container.
        /// </summary>
        /// <returns>Returns the current instance</returns>
        public DependencyContainer Initialize()
        {
            if (this._isInitialized)
                throw new InvalidOperationException("Dependency container is already initialized.");

            var services = ReflectionHelper.GetClassesWithCustomAttribute<ServiceAttribute>();

            foreach (var serviceType in services)
            {
                var serviceInterface = serviceType.GetInterfaces().First();

                this._services.AddTransient(serviceInterface, serviceType);
            }
            

            this._isInitialized = true;

            return this;
        }

        public DependencyContainer Initialize(IServiceCollection serviceCollection)
        {
            this._services = serviceCollection;

            return this.Initialize();
        }

        /// <summary>
        /// Build the service provider of the dependency container.
        /// </summary>
        /// <returns></returns>
        public IServiceProvider BuildServiceProvider()
        {
            if (!this._isInitialized)
                throw new InvalidOperationException("Dependency container is not initialized. Please call the Initialize() method first.");

            if (this._serviceProvider == null)
                this._serviceProvider = this._services.BuildServiceProvider();

            return this._serviceProvider;
        }

        /// <summary>
        /// Gets the service collection.
        /// </summary>
        /// <returns></returns>
        public IServiceCollection GetServiceCollection() => this._services;

        /// <summary>
        /// Resolve a dependency.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>()
        {
            if (this._serviceProvider == null)
                throw new InvalidOperationException("Cannot resolve dependency. Service Provider has not been initialized. Please call the BuildServiceProvider() method.");

            return this._serviceProvider.GetService<T>();
        }
    }
}
