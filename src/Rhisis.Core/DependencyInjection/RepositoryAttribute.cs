using Microsoft.Extensions.DependencyInjection;
using System;

namespace Rhisis.Core.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class RepositoryAttribute : ServiceAttribute
    {
        /// <summary>
        /// Creates a new <see cref="RepositoryAttribute"/> instance.
        /// </summary>
        /// <param name="serviceLifeTime"></param>
        public RepositoryAttribute(ServiceLifetime serviceLifeTime = ServiceLifetime.Transient)
            : base(serviceLifeTime)
        {
        }
    }
}
