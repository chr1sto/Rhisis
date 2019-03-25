using System;

namespace Rhisis.Core.Resources
{
    /// <summary>
    /// Provides a mechanism to load a game resource.
    /// </summary>
    public interface IGameResourceLoader : IDisposable
    {
        /// <summary>
        /// Load a game resource.
        /// </summary>
        void Load();
    }
}
