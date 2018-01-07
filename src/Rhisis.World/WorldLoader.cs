using Rhisis.Core.IO;
using Rhisis.Core.Resources;
using Rhisis.Core.Structures.Game;
using Rhisis.World.Core.Systems;
using Rhisis.World.Game;
using Rhisis.World.Game.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Rhisis.World.Game.Chat;
using Rhisis.World.Systems;

namespace Rhisis.World
{
    public partial class WorldServer
    {
        private static readonly string DataPath = Path.Combine(Directory.GetCurrentDirectory(), "data");
        private static readonly string ResourcePath = Path.Combine(DataPath, "res");
        private static readonly IDictionary<string, int> Defines = new Dictionary<string, int>();
        private static readonly IDictionary<string, string> Texts = new Dictionary<string, string>();
        private static readonly IDictionary<int, MoverData> MoversData = new Dictionary<int, MoverData>();
        private static readonly IDictionary<int, ItemData> ItemsData = new Dictionary<int, ItemData>();

        /// <summary>
        /// Gets the Movers data.
        /// </summary>
        public static IReadOnlyDictionary<int, MoverData> Movers => MoversData as IReadOnlyDictionary<int, MoverData>;

        /// <summary>
        /// Gets the Items data.
        /// </summary>
        public static IReadOnlyDictionary<int, ItemData> Items => ItemsData as IReadOnlyDictionary<int, ItemData>;

        /// <summary>
        /// Loads the server's resources.
        /// </summary>
        private void LoadResources()
        {
            Logger.Info("Loading resources...");
            Profiler.Start("LoadResources");

            this.LoadDefinesAndTexts();
            this.LoadMovers();
            this.LoadItems();
            this.LoadMaps();
            this.CleanUp();

            var time = Profiler.Stop("LoadResources");
            Logger.Info("Resources loaded in {0}ms", time.ElapsedMilliseconds);
        }

        private void LoadDefinesAndTexts()
        {
            var headerFiles = from x in Directory.GetFiles(ResourcePath, "*.*", SearchOption.AllDirectories)
                              where DefineFile.Extensions.Contains(Path.GetExtension(x))
                              select x;

            var textFiles = from x in Directory.GetFiles(ResourcePath, "*.*", SearchOption.AllDirectories)
                            where TextFile.Extensions.Contains(Path.GetExtension(x)) && x.EndsWith(".txt.txt")
                            select x;

            foreach (var headerFile in headerFiles)
            {
                using (var defineFile = new DefineFile(headerFile))
                {
                    foreach (var define in defineFile.Defines)
                    {
                        if (!Defines.ContainsKey(define.Key) && define.Value is int)
                            Defines.Add(define.Key, int.Parse(define.Value.ToString()));
                    }
                }
            }

            foreach (var textFilePath in textFiles)
            {
                using (var textFile = new TextFile(textFilePath))
                {
                    foreach (var text in textFile.Texts)
                    {
                        if (!Texts.ContainsKey(text.Key) && !string.IsNullOrEmpty(text.Value))
                            Texts.Add(text);
                    }
                }
            }
        }

        private void LoadMovers()
        {
            string propMoverPath = Path.Combine(ResourcePath, "data", "propMover.txt");

            Logger.Loading("Loading movers...");
            using (var propMoverFile = new ResourceTable(propMoverPath, 1, Defines, Texts))
            {
                var movers = propMoverFile.GetRecords<MoverData>();

                foreach (var mover in movers)
                {
                    if (MoversData.ContainsKey(mover.Id))
                        MoversData[mover.Id] = mover;
                    else
                        MoversData.Add(mover.Id, mover);
                }
            }
            Logger.Info("{0} movers loaded!\t\t", MoversData.Count);
        }

        private void LoadItems()
        {
            string propItemPath = Path.Combine(ResourcePath, "dataSub2", "propItem.txt");

            Logger.Loading("Loading items...");
            using (var propItem = new ResourceTable(propItemPath, 1, Defines, Texts))
            {
                var items = propItem.GetRecords<ItemData>();

                foreach (var item in items)
                {
                    if (ItemsData.ContainsKey(item.Id))
                        ItemsData[item.Id] = item;
                    else
                        ItemsData.Add(item.Id, item);
                }
            }
            Logger.Info("{0} items loaded!\t\t", ItemsData.Count);
        }

        private void LoadMaps()
        {
            Logger.Loading("Loading maps...\t\t");
            IEnumerable<Type> systemTypes = this.LoadSystems();
            IDictionary<string, string> worldsPaths = this.LoadWorldScript();

            foreach (string mapId in this.WorldConfiguration.Maps)
            {
                if (!worldsPaths.TryGetValue(mapId, out string mapName))
                {
                    Logger.Warning("Cannot load map with Id: {0}. Please check your world script file.", mapId);
                    continue;
                }

                if (!Defines.TryGetValue(mapId, out int id))
                {
                    Logger.Warning("Cannot find map Id in define files: {0}. Please check you defineWorld.h file.",
                        mapId);
                    continue;
                }

                Map map = Map.Load(Path.Combine(DataPath, "maps", mapName), mapName, id);

                foreach (Type type in systemTypes)
                    map.Context.AddSystem(Activator.CreateInstance(type, map.Context) as ISystem);

                map.Start();
                _maps.Add(id, map);
            }

            Logger.Info("{0} maps loaded! \t\t", _maps.Count);
        }

        private IEnumerable<Type> LoadSystems()
        {
            ChatSystem.InitializeCommands();

            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.GetTypeInfo().GetCustomAttribute<SystemAttribute>() != null && typeof(ISystem).IsAssignableFrom(x));
        }

        private IDictionary<string, string> LoadWorldScript()
        {
            var worldsPaths = new Dictionary<string, string>();

            using (var textFile = new TextFile(Path.Combine(ResourcePath, "data", "World.inc")))
            {
                foreach (var text in textFile.Texts)
                    worldsPaths.Add(text.Key, text.Value.Replace('"', ' ').Trim());
            }

            return worldsPaths;
        }

        private void CleanUp()
        {
            Defines.Clear();
            Texts.Clear();
        }
    }
}