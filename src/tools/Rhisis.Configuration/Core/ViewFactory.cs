using System;
using System.Collections.Generic;

namespace Rhisis.Configuration.Core
{
    public static class ViewFactory
    {
        private static readonly IDictionary<Type, Type> _types = new Dictionary<Type, Type>();

        public static void Register<TView, TViewModel>()
        {
            if (_types.ContainsKey(typeof(TViewModel)))
                throw new ArgumentException($"The ViewModel '{typeof(TViewModel).Name}' alreaady exists in ViewFactory.");

            _types.Add(typeof(TViewModel), typeof(TView));
        }

        public static object CreateInstance<TViewModel>()
        {
            return CreateInstance(typeof(TViewModel));
        }

        public static object CreateInstance(Type viewModelType)
        {
            if (_types.ContainsKey(viewModelType))
            {
                var type = _types[viewModelType];

                return Activator.CreateInstance(type);
            }

            throw new KeyNotFoundException();
        }
    }
}
