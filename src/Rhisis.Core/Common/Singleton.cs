using System;

namespace Rhisis.Core.Common
{
    /// <summary>
    /// Lazy singleton implementation.
    /// </summary>
    /// <typeparam name="T">Singleton type</typeparam>
    public class Singleton<T> where T : class, new()
    {
        private static readonly Lazy<T> _instance = new Lazy<T>(() => new T());

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static T Instance => _instance.Value;

        /// <summary>
        /// Creates a new <see cref="Singleton{T}"/> internally.
        /// </summary>
        protected Singleton()
        {
        }
    }
}
