using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Rhisis.Core.Handlers.Internal
{
    /// <summary>
    /// Provides methods to invoke a handler action.
    /// </summary>
    internal sealed class HandlerActionInvoker : IHandlerInvoker
    {
        private readonly HandlerActionInvokerCache _invokerCache;

        /// <summary>
        /// Creates a new <see cref="HandlerActionInvoker"/> instance.
        /// </summary>
        /// <param name="invokerCache">Handler action invoker cache.</param>
        public HandlerActionInvoker(HandlerActionInvokerCache invokerCache)
        {
            this._invokerCache = invokerCache;
        }

        /// <inheritdoc />
        public object Invoke(object handlerAction, params object[] args)
        {
            HandlerActionInvokerCacheEntry handlerActionInvoker = this._invokerCache.GetCachedHandlerAction(handlerAction);

            if (handlerActionInvoker == null)
                throw new ArgumentNullException(nameof(handlerActionInvoker));

            var targetHandler = handlerActionInvoker.HandlerFactory(handlerActionInvoker.HandlerType);

            if (targetHandler == null)
                throw new ArgumentNullException(nameof(targetHandler));

            object handlerResult = null;

            try
            {
                var handlerActionParameters = this.PrepareParameters(args, handlerActionInvoker.HandlerExecutor);

                handlerResult = handlerActionInvoker.HandlerExecutor.Execute(targetHandler, handlerActionParameters);
            }
            catch
            {
                throw;
            }
            finally
            {
                handlerActionInvoker.HandlerReleaser(targetHandler);
            }

            return handlerResult;
        }

        /// <inheritdoc />
        public Task InvokeAsync(object handlerAction, params object[] args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Prepare the invoke parameters. Adds default values if a parameter is missing.
        /// </summary>
        /// <param name="originalParameters">Original parameters.</param>
        /// <param name="executor">Handler executor.</param>
        /// <returns>Handler parameters.</returns>
        private object[] PrepareParameters(object[] originalParameters, HandlerExecutor executor)
        {
            if (!executor.MethodParameters.Any())
                return null;

            var parameters = new object[executor.MethodParameters.Count()];

            for (var i = 0; i < parameters.Length; i++)
            {
                ParameterInfo methodParameterInfo = executor.MethodParameters.ElementAt(i);

                if (i < originalParameters.Length)
                {
                    if (originalParameters[i] != null && !methodParameterInfo.ParameterType.IsAssignableFrom(originalParameters[i].GetType()))
                        parameters[i] = executor.GetDefaultValueForParameter(i);
                    else
                        parameters[i] = originalParameters[i];
                }
                else
                {
                    parameters[i] = executor.GetDefaultValueForParameter(i);
                }
            }

            return parameters;
        }
    }
}
