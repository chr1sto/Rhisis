using Newtonsoft.Json.Linq;
using Rhisis.Core.IO;
using Rhisis.Core.Resources;
using Rhisis.Core.Resources.Include;
using Rhisis.Core.Structures.Game;
using Rhisis.World.Game.Behaviors;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Maps;
using Rhisis.World.Systems.Chat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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
        private static readonly IDictionary<string, NpcData> NpcData = new Dictionary<string, NpcData>();
        private static readonly IDictionary<string, ShopData> ShopData = new Dictionary<string, ShopData>();
        private static readonly IDictionary<string, DialogData> DialogData = new Dictionary<string, DialogData>();

        public static readonly BehaviorManager<Game.Entities.IMonsterEntity> MonsterBehaviors = new BehaviorManager<Game.Entities.IMonsterEntity>(BehaviorType.Monster);
        public static readonly BehaviorManager<Game.Entities.INpcEntity> NpcBehaviors = new BehaviorManager<Game.Entities.INpcEntity>(BehaviorType.Npc);
        public static readonly BehaviorManager<Game.Entities.IPlayerEntity> PlayerBehaviors = new BehaviorManager<Game.Entities.IPlayerEntity>(BehaviorType.Player);

        /// <summary>
        /// Gets the Movers data.
        /// </summary>
        public static IReadOnlyDictionary<int, MoverData> Movers => MoversData as IReadOnlyDictionary<int, MoverData>;

        /// <summary>
        /// Gets the Items data.
        /// </summary>
        public static IReadOnlyDictionary<int, ItemData> Items => ItemsData as IReadOnlyDictionary<int, ItemData>;

        /// <summary>
        /// Gets the Npcs data.
        /// </summary>
        public static IReadOnlyDictionary<string, NpcData> Npcs => NpcData as IReadOnlyDictionary<string, NpcData>;

        /// <summary>
        /// Loads the server's resources.
        /// </summary>
        private void LoadResources()
        {
            Logger.Info("Loading resources...");
            Profiler.Start("LoadResources");

            this.LoadDefinesAndTexts();
            this.LoadMovers();
            this.LoadBehaviors();
            this.LoadItems();
            this.LoadNpc();
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

        private void LoadNpc()
        {
            IEnumerable<string> files = from x in Directory.GetFiles(ResourcePath, "*.*", SearchOption.AllDirectories)
                                        where Path.GetFileName(x).StartsWith("character") && x.EndsWith(".inc")
                                        select x;

            Logger.Loading("Loading npcs...");

            this.LoadNpcShops();
            this.LoadNpcDialogs();

            foreach (string file in files)
            {
                using (var npcFile = new IncludeFile(file))
                {
                    foreach (IStatement npcStatement in npcFile.Statements)
                    {
                        if (!(npcStatement is Block npcBlock))
                            continue;

                        string npcId = npcStatement.Name;
                        string npcName = npcId;

                        foreach (IStatement npcInfoStatement in npcBlock.Statements)
                        {
                            if (npcInfoStatement is Instruction instruction && npcInfoStatement.Name == "SetName")
                            {
                                if (instruction.Parameters.Count > 0 && 
                                    Texts.TryGetValue(instruction.Parameters.First().ToString(), out string value))
                                {
                                    npcName = value;
                                }
                            }
                        }

                        ShopData.TryGetValue(npcId, out ShopData shop);
                        DialogData.TryGetValue(npcId, out DialogData dialog);

                        var newNpc = new NpcData(npcId, npcName, shop, dialog);

                        if (!NpcData.ContainsKey(newNpc.Id))
                            NpcData.Add(newNpc.Id, newNpc);
                        else
                            NpcData[newNpc.Id] = newNpc;
                    }
                }
            }

            Logger.Info("{0} npcs loaded!\t\t", NpcData.Count);
        }

        private void LoadNpcShops()
        {
            string shopsPath = Path.Combine(DataPath, "shops");

            if (!Directory.Exists(shopsPath))
            {
                Logger.Warning("Cannot find {0} directory.", shopsPath);
                return;
            }

            string[] shopsFiles = Directory.GetFiles(shopsPath);

            foreach (string shopFile in shopsFiles)
            {
                string shopFileContent = File.ReadAllText(shopFile);
                JToken shopsParsed = JToken.Parse(shopFileContent, new JsonLoadSettings
                {
                    CommentHandling = CommentHandling.Ignore
                });

                if (shopsParsed.Type == JTokenType.Array)
                {
                    var shops = shopsParsed.ToObject<ShopData[]>();

                    foreach (ShopData shop in shops)
                        if (!ShopData.ContainsKey(shop.Name))
                            ShopData.Add(shop.Name, shop);
                }
                else
                {
                    var shop = shopsParsed.ToObject<ShopData>();

                    if (!ShopData.ContainsKey(shop.Name))
                        ShopData.Add(shop.Name, shop);
                }
            }
        }

        private void LoadNpcDialogs()
        {
            string dialogsPath = Path.Combine(DataPath, "dialogs", this.WorldConfiguration.Language);

            if (!Directory.Exists(dialogsPath))
            {
                Logger.Warning("Cannot find {0} directory.", dialogsPath);
                return;
            }

            string[] dialogFiles = Directory.GetFiles(dialogsPath);

            foreach (string dialogFile in dialogFiles)
            {
                string dialogFileContent = File.ReadAllText(dialogFile);
                JToken dialogsParsed = JToken.Parse(dialogFileContent, new JsonLoadSettings
                {
                    CommentHandling = CommentHandling.Ignore
                });

                if (dialogsParsed.Type == JTokenType.Array)
                {
                    var dialogs = dialogsParsed.ToObject<DialogData[]>();

                    foreach (DialogData dialog in dialogs)
                        if (!DialogData.ContainsKey(dialog.Name))
                            DialogData.Add(dialog.Name, dialog);
                }
                else
                {
                    var dialog = dialogsParsed.ToObject<DialogData>();

                    if (!DialogData.ContainsKey(dialog.Name))
                        DialogData.Add(dialog.Name, dialog);
                }
            }
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
            IReadOnlyDictionary<string, string> worldsPaths = this.LoadWorldScript();

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

                IMapInstance map = MapInstance.Create(Path.Combine(DataPath, "maps", mapName), mapName, id);

                if (_maps.ContainsKey(id))
                {
                    Logger.Error("Cannot create map {0} with id {1} because it already exist.", mapName, id);
                    continue;
                }
                
                foreach (Type type in systemTypes)
                    map.AddSystem(Activator.CreateInstance(type, map) as ISystem);

                _maps.Add(id, map);
                map.StartUpdateTask(100);
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

        private IReadOnlyDictionary<string, string> LoadWorldScript()
        {
            var worldsPaths = new Dictionary<string, string>();

            using (var textFile = new TextFile(Path.Combine(ResourcePath, "data", "World.inc")))
            {
                foreach (var text in textFile.Texts)
                    worldsPaths.Add(text.Key, text.Value.Replace('"', ' ').Trim());
            }

            return worldsPaths;
        }

        private void LoadBehaviors()
        {
            Logger.Loading("Loading behaviors...\t\t");
            
            MonsterBehaviors.Load();
            NpcBehaviors.Load();
            PlayerBehaviors.Load();

            int totalBehaviorsLoaded = MonsterBehaviors.Count + NpcBehaviors.Count + PlayerBehaviors.Count;

            Logger.Info("{0} behaviors loaded!\t\t", totalBehaviorsLoaded);
        }

        private void CleanUp()
        {
            Defines.Clear();
            Texts.Clear();
            ShopData.Clear();
            GC.Collect();
        }
    }
}