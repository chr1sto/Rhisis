using Rhisis.Core.DependencyInjection;
using System;
using Xunit;

namespace Rhisis.Core.Test.DependencyInjection
{
    public class DependencyContainerTest : IDisposable
    {
        private readonly DependencyContainer _dependencyContainer;

        public DependencyContainerTest()
        {
            this._dependencyContainer = new DependencyContainer();
        }

        [Fact(DisplayName = "Register services with generic method")]
        public void RegisterServicesGenerics()
        {
            this._dependencyContainer.Register<IServiceA, ServiceA>();
            this._dependencyContainer.Register<IServiceB, ServiceB>();

            Assert.Equal(2, this._dependencyContainer.Count);
        }

        [Fact(DisplayName = "Register services with typed method")]
        public void RegisterServices()
        {
            this._dependencyContainer.Register(typeof(IServiceA), typeof(ServiceA));
            this._dependencyContainer.Register(typeof(IServiceB), typeof(ServiceB));

            Assert.Equal(2, this._dependencyContainer.Count);
        }

        [Fact(DisplayName = "Resolve service with typed registration")]
        public void ResolveServiceWithTypedRegister()
        {
            this._dependencyContainer.Register(typeof(IServiceA), typeof(ServiceA));
            this._dependencyContainer.Register(typeof(IServiceB), typeof(ServiceB));
            this._dependencyContainer.BuildServiceProvider();
            
            var serviceA = this._dependencyContainer.Resolve<IServiceA>();

            Assert.Equal(2, this._dependencyContainer.Count);
            Assert.NotNull(serviceA);
            Assert.IsAssignableFrom<IServiceA>(serviceA);
            Assert.IsType<ServiceA>(serviceA);
        }

        [Fact(DisplayName = "Resolve service with generic registration")]
        public void ResolveServiceWithGenericRegister()
        {
            this._dependencyContainer.Register<IServiceA, ServiceA>();
            this._dependencyContainer.Register<IServiceB, ServiceB>();
            this._dependencyContainer.BuildServiceProvider();
            
            var serviceA = this._dependencyContainer.Resolve<IServiceA>();

            Assert.Equal(2, this._dependencyContainer.Count);
            Assert.NotNull(serviceA);
            Assert.IsAssignableFrom<IServiceA>(serviceA);
            Assert.IsType<ServiceA>(serviceA);
        }

        [Fact(DisplayName = "Resolve service with generic and typed register")]
        public void ResolveServiceWithGenericAndTypedRegister()
        {
            this._dependencyContainer.Register<IServiceA, ServiceA>();
            this._dependencyContainer.Register(typeof(IServiceB), typeof(ServiceB));
            this._dependencyContainer.BuildServiceProvider();

            var serviceA = this._dependencyContainer.Resolve<IServiceA>();
            var serviceB = this._dependencyContainer.Resolve<IServiceB>();

            Assert.Equal(2, this._dependencyContainer.Count);
            Assert.NotNull(serviceA);
            Assert.IsAssignableFrom<IServiceA>(serviceA);
            Assert.IsType<ServiceA>(serviceA);
            Assert.NotNull(serviceB);
            Assert.IsAssignableFrom<IServiceB>(serviceB);
            Assert.IsType<ServiceB>(serviceB);
        }

        [Fact(DisplayName = "Resolve service without building provider")]
        public void ResolveServiceWithoutBuildProvider()
        {
            this._dependencyContainer.Register(typeof(IServiceA), typeof(ServiceA));
            this._dependencyContainer.Register(typeof(IServiceB), typeof(ServiceB));

            var exception = Record.Exception(() => { this._dependencyContainer.Resolve<IServiceA>(); });

            Assert.Equal(2, this._dependencyContainer.Count);
            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
        }

        [Fact(DisplayName = "Resolve unknown service")]
        public void ResolveUnknownService()
        {
            this._dependencyContainer.Register(typeof(IServiceB), typeof(ServiceB));
            this._dependencyContainer.BuildServiceProvider();

            var serviceA = this._dependencyContainer.Resolve<IServiceA>();

            Assert.Equal(1, this._dependencyContainer.Count);
            Assert.Null(serviceA);
        }

        [Fact(DisplayName = "Resolve service without building provider")]
        public void ResolveUnknownServiceWithoutBuildProvider()
        {
            this._dependencyContainer.Register(typeof(IServiceB), typeof(ServiceB));

            var exception = Record.Exception(() => { this._dependencyContainer.Resolve<IServiceA>(); });

            Assert.Equal(1, this._dependencyContainer.Count);
            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
        }

        public void Dispose()
        {
            this._dependencyContainer.Dispose();
        }
    }

    public interface IServiceA
    {
    }

    public interface IServiceB
    {
    }

    public class ServiceA : IServiceA
    {
    }

    public class ServiceB : IServiceB
    {
    }
}