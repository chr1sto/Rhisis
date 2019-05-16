using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.Common;
using System;

namespace Rhisis.Core.DependencyInjection
{
    public sealed class DependencyContainer : Singleton<DependencyContainer>, IDisposable
    {
        private IServiceCollection _services;
        private ServiceProvider _serviceProvider;

        /// <summary>
        /// Gets the number of registered services.
        /// </summary>
        public int Count => this._services.Count;

        /// <summary>
        /// Creates a new <see cref="DependencyContainer"/> instance.
        /// </summary>
        public DependencyContainer()
        {
            this._services = new ServiceCollection();
        }

        /// <summary>
        /// Sets the dependency container's service collection.
        /// </summary>
        /// <param name="serviceCollection">Existing service collection</param>
        /// <returns></returns>
        public DependencyContainer SetServiceCollection(IServiceCollection serviceCollection)
        {
            this._services = serviceCollection;
            return this;
        }

        /// <summary>
        /// Build the service provider of the dependency container.
        /// </summary>
        /// <returns></returns>
        public IServiceProvider BuildServiceProvider()
        {
            if (this._serviceProvider == null && this._services != null)
                this._serviceProvider = this._services.BuildServiceProvider();

            return this._serviceProvider;
        }

        /// <summary>
        /// Gets the service collection.
        /// </summary>
        /// <returns></returns>
        public IServiceCollection GetServiceCollection() => this._services;
        
        /// <summary>
        /// Register a new service.
        /// </summary>
        /// <param name="implementationType">Service implementation type</param>
        /// <param name="serviceType">Service type</param>
        /// <param name="serviceLifetime">Service life time</param>
        public void Register(Type implementationType, Type serviceType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            if (this._services == null)
                throw new InvalidOperationException("Cannot register dependency. ServiceCollection has not been initialized. Please call the Initialize() method.");

            Func<Type, Type, IServiceCollection> addServiceMethod;

            switch (serviceLifetime)
            {
                case ServiceLifetime.Singleton:
                    addServiceMethod = this._services.AddSingleton;
                    break;

                case ServiceLifetime.Scoped:
                    addServiceMethod = this._services.AddScoped;
                    break;

                case ServiceLifetime.Transient:
                default:
                    addServiceMethod = this._services.AddTransient;
                    break;
            }

            addServiceMethod(implementationType, serviceType);
        }
        
        /// <summary>
        /// Register a new service.
        /// </summary>
        /// <typeparam name="TImplementation">Service implementation type</typeparam>
        /// <typeparam name="TService">Service type</typeparam>
        /// <param name="serviceLifetime">Service life time</param>
        public void Register<TImplementation, TService>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient) 
            => this.Register(typeof(TImplementation), typeof(TService), serviceLifetime);

        /// <summary>
        /// Registers a service without interface.
        /// </summary>
        /// <param name="serviceType">Service type</param>
        /// <param name="serviceLifetime">Service life time</param>
        public void Register(Type serviceType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            if (this._services == null)
                throw new InvalidOperationException("Cannot register dependency. ServiceCollection has not been initialized. Please call the Initialize() method.");

            Func<Type, IServiceCollection> addServiceMethod;

            switch (serviceLifetime)
            {
                case ServiceLifetime.Singleton:
                    addServiceMethod = this._services.AddSingleton;
                    break;

                case ServiceLifetime.Scoped:
                    addServiceMethod = this._services.AddScoped;
                    break;

                case ServiceLifetime.Transient:
                default:
                    addServiceMethod = this._services.AddTransient;
                    break;
            }

            addServiceMethod(serviceType);
        }

        /// <summary>
        /// Registers a service.
        /// </summary>
        /// <typeparam name="TService">Service type</typeparam>
        /// <param name="serviceLifetime">Service life time</param>
        public void Register<TService>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
            => this.Register(typeof(TService), serviceLifetime);
        
        /// <summary>
        /// Resolve a dependency.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>() where T : class => this.Resolve(typeof(T)) as T;

        /// <summary>
        /// Resolves a dependency.
        /// </summary>
        /// <param name="type">Dependency type to resolve.</param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            if (this._serviceProvider == null)
                throw new InvalidOperationException("Cannot resolve dependency. Service Provider has not been initialized. Please call the BuildServiceProvider() method.");

            return this._serviceProvider.GetService(type);
        }

        /// <summary>
        /// Configure the <see cref="DependencyContainer"/>.
        /// </summary>
        /// <param name="serviceBuilder"></param>
        public void Configure(Action<IServiceCollection> serviceBuilder) => serviceBuilder(this._services);

        /// <summary>
        /// Dispose the dependency container's resources.
        /// </summary>
        public void Dispose()
        {
            this._services.Clear();

            if (this._serviceProvider != null)
            {
                this._serviceProvider.Dispose();
                this._serviceProvider = null;
            }
        }
    }
}
