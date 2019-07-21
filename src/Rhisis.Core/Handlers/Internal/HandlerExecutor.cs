using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Rhisis.Core.Handlers.Internal
{
    internal sealed class HandlerExecutor
    {
        private delegate object HandlerMethodExecutor(object target, object[] parameters);
        private delegate void VoidHandlerMethodExecutor(object target, object[] parameters);

        private readonly TypeInfo _targetHandlerTypeInfo;
        private readonly MethodInfo _handlerActionMethodInfo;
        private readonly object[] _handlerActionDefaultParameters;
        private readonly HandlerMethodExecutor _executor;

        public ParameterInfo[] MethodParameters { get; }

        public HandlerExecutor(TypeInfo targetHandlerTypeInfo, MethodInfo handlerActionMethodInfo, object[] defaultParameters)
        {
            this._targetHandlerTypeInfo = targetHandlerTypeInfo;
            this._handlerActionMethodInfo = handlerActionMethodInfo;
            this._handlerActionDefaultParameters = defaultParameters;
            this._executor = this.BuildExecutor();

            this.MethodParameters = handlerActionMethodInfo.GetParameters();
        }

        public object Execute(object target, params object[] parameters) => this._executor(target, parameters);

        private HandlerMethodExecutor BuildExecutor()
        {
            ParameterExpression targetParameter = Expression.Parameter(typeof(object), "target");
            ParameterExpression parametersParameter = Expression.Parameter(typeof(object[]), "parameters");

            var parameters = new List<Expression>();
            ParameterInfo[] parameterInfos = this._handlerActionMethodInfo.GetParameters();

            for (var i = 0; i < parameterInfos.Length; i++)
            {
                ParameterInfo paramInfo = parameterInfos[i];
                BinaryExpression valueObj = Expression.ArrayIndex(parametersParameter, Expression.Constant(i));
                UnaryExpression valueCast = Expression.Convert(valueObj, paramInfo.ParameterType);

                parameters.Add(valueCast);
            }

            UnaryExpression instanceCast = Expression.Convert(targetParameter, this._targetHandlerTypeInfo.AsType());
            MethodCallExpression methodCall = Expression.Call(instanceCast, this._handlerActionMethodInfo, parameters);

            if (methodCall.Type == typeof(void))
            {
                var executor = Expression.Lambda<VoidHandlerMethodExecutor>(methodCall, targetParameter, parametersParameter).Compile();

                return (target, args) =>
                {
                    executor(target, args);
                    return null;
                };
            }
            else
            {
                UnaryExpression castMethodCall = Expression.Convert(methodCall, typeof(object));

                return Expression.Lambda<HandlerMethodExecutor>(castMethodCall, targetParameter, parametersParameter).Compile();
            }
        }

        public object GetDefaultValueForParameter(int index)
        {
            if (index < 0 || index > this.MethodParameters.Length - 1)
                throw new ArgumentOutOfRangeException(nameof(index));

            return this._handlerActionDefaultParameters[index];
        }
    }
}
