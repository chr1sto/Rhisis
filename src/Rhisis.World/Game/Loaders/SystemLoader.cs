using Rhisis.Core.Resources;
using Rhisis.World.Game.Core.Systems;
using Rhisis.World.Systems.Chat;

namespace Rhisis.World.Game.Loaders
{
    public sealed class SystemLoader : IGameResourceLoader
    {
        /// <inheritdoc />
        public void Load()
        {
            ChatSystem.Initialize();
            SystemManager.Instance.Initialize();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // Nothing to dispose.
        }
    }
}
