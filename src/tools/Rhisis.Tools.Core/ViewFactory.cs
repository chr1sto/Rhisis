using System;
using System.Collections.Generic;

namespace Rhisis.Tools.Core
{
    public static class ViewFactory
    {
        private static readonly IDictionary<Type, Type> _types = new Dictionary<Type, Type>();

        /// <summary>
        /// Register a ViewModel with a View.
        /// </summary>
        /// <typeparam name="TView">View type</typeparam>
        /// <typeparam name="TViewModel">ViewModel type</typeparam>
        public static void Register<TView, TViewModel>()
        {
            if (_types.ContainsKey(typeof(TViewModel)))
                throw new ArgumentException($"The ViewModel '{typeof(TViewModel).Name}' alreaady exists in ViewFactory.");

            _types.Add(typeof(TViewModel), typeof(TView));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TViewModel">ViewModel type</typeparam>
        /// <returns></returns>
        public static object CreateInstance<TViewModel>()
        {
            return CreateInstance(typeof(TViewModel));
        }

        /// <summary>
        /// Creates an instances of the type associated to the ViewModel.
        /// </summary>
        /// <param name="viewModelType">ViewModel type</param>
        /// <returns></returns>
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
