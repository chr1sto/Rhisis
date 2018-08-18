using System;

namespace Rhisis.Core.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class RepositoryAttribute : Attribute
    {
    }
}
