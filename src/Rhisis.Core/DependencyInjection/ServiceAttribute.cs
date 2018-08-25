using System;
using Microsoft.Extensions.DependencyInjection;

namespace Rhisis.Core.DependencyInjection
{
    /// <summary>
    /// Attribute that describes a class has a injectable service.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ServiceAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the service life time.
        /// </summary>
        public ServiceLifetime LifeTime { get; set; }

        /// <summary>
        /// Creates a new <see cref="ServiceAttribute"/> instance.
        /// </summary>
        /// <param name="serviceLifeTime"></param>
        public ServiceAttribute(ServiceLifetime serviceLifeTime = ServiceLifetime.Transient)
        {
            this.LifeTime = serviceLifeTime;
        }
    }
}
