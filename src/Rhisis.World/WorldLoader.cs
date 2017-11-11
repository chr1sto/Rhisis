using Rhisis.Core.IO;
using Rhisis.World.Game;
using Rhisis.World.Systems;

namespace Rhisis.World
{
    public partial class WorldServer
    {
        private void LoadResources()
        {
            Logger.Info("Loading resources...");
            Profiler.Start("LoadResources");

            this.LoadSystems();
            this.LoadMaps();

            var time = Profiler.Stop("LoadResources");
            Logger.Info("Resources loaded in {0}ms", time.ElapsedMilliseconds);
        }

        private void LoadSystems()
        {
            Logger.Loading("Loading systems...");

            // TODO: Load systems using reflection

            Logger.Info("Systems loaded! \t\t");
        }

        private void LoadMaps()
        {
            Logger.Loading("Loading maps...\t\t");

            var map = Map.Load("data/maps/WdMadrigal"); // Load map
            map.Context.AddSystem(new VisibilitySystem(map.Context));
            map.Context.AddSystem(new MobilitySystem(map.Context));
            map.Start(); // Start map update thread

            _maps.Add(1, map); // Add the map to the 

            Logger.Info("All maps loaded! \t\t");
        }
    }
}
