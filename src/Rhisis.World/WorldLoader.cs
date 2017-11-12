using Hellion.Core.Resources;
using Rhisis.Core.IO;
using Rhisis.World.Game;
using Rhisis.World.Systems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rhisis.World
{
    public partial class WorldServer
    {
        private static readonly string DataPath = Path.Combine(Directory.GetCurrentDirectory(), "data");
        private static readonly string ResourcePath = Path.Combine(DataPath, "res");
        private static readonly IDictionary<string, int> _defines = new Dictionary<string, int>();
        private static readonly IDictionary<string, string> _texts = new Dictionary<string, string>();

        private void LoadResources()
        {
            Logger.Info("Loading resources...");
            Profiler.Start("LoadResources");

            this.LoadDefines();
            this.LoadSystems();
            this.LoadMaps();
            this.CleanUp();

            var time = Profiler.Stop("LoadResources");
            Logger.Info("Resources loaded in {0}ms", time.ElapsedMilliseconds);
        }

        private void LoadDefines()
        {
            var headerFiles = from x in Directory.GetFiles(ResourcePath, "*.*", SearchOption.AllDirectories)
                              where DefineFile.Extensions.Contains(Path.GetExtension(x))
                              select x;

            foreach (var headerFile in headerFiles)
            {
                using (var defineFile = new DefineFile(headerFile))
                {
                    foreach (var define in defineFile.Defines)
                    {
                        if (!_defines.ContainsKey(define.Key) && define.Value is int)
                            _defines.Add(define.Key, int.Parse(define.Value.ToString()));
                    }
                }
            }
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
            map.Context.AddSystem(new ChatSystem(map.Context));
            map.Start(); // Start map update thread

            _maps.Add(1, map); // Add the map to the 

            Logger.Info("All maps loaded! \t\t");
        }

        private void CleanUp()
        {
            _defines.Clear();
            _texts.Clear();

            GC.Collect();
        }
    }
}
