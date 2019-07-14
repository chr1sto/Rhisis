using System;

namespace Rhisis.Core.Handlers.Internal
{
    /// <summary>
    /// Provides methods to create and release handlers.
    /// </summary>
    internal sealed class HandlerFactory : IHandlerFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ITypeActivatorCache _typeActivatorCache;

        /// <summary>
        /// Creates a new <see cref="HandlerFactory"/> instance.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="typeActivatorCache"></param>
        public HandlerFactory(IServiceProvider serviceProvider, ITypeActivatorCache typeActivatorCache)
        {
            this._serviceProvider = serviceProvider;
            this._typeActivatorCache = typeActivatorCache;
        }

        /// <inheritdoc />
        public object CreateHandler(Type handlerType)
        {
            if (handlerType == null)
                throw new ArgumentNullException(nameof(handlerType));

            return this._typeActivatorCache.Create<object>(this._serviceProvider, handlerType);
        }

        /// <inheritdoc />
        public void ReleaseHandler(object handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            if (handler is IDisposable disposableHandler)
                disposableHandler.Dispose();
        }
    }
}
