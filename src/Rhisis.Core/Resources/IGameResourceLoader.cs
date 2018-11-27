using System;

namespace Rhisis.Core.Resources
{
    public interface IGameResourceLoader : IDisposable
    {
        void Load();
    }
}
