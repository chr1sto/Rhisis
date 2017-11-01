using Rhisis.Core.IO;
using Rhisis.World.Game;

namespace Rhisis.World
{
    public partial class WorldServer
    {
        private void LoadResources()
        {
            this.LoadMaps();
        }

        private void LoadMaps()
        {
            Logger.Loading("Loading maps...");

            var map = Map.Load("data/maps/WdMadrigal"); // Load map
            map.Start(); // Start map update thread

            _maps.Add(1, map); // Add the map to the 

            Logger.Info("All maps loaded!");
        }
    }
}
