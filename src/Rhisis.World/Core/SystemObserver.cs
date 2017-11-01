using Rhisis.World.Core.Systems;
using System.Collections.Generic;

namespace Rhisis.World.Core
{
    public static class SystemObserver
    {
        private static readonly ICollection<ISystem> _systems = new List<ISystem>();

        public static void AddSystem(ISystem system)
        {
        }

        public static void DeleteSystem(ISystem system)
        {
        }

        public static void NotifySystems()
        {
        }
    }
}
