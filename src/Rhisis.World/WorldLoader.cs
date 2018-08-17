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
        // Paths
        private static readonly string DataPath = Path.Combine(Directory.GetCurrentDirectory(), "data");
        private static readonly string DialogsPath = Path.Combine(DataPath, "dialogs");
        private static readonly string ResourcePath = Path.Combine(DataPath, "res");
        private static readonly string MapsPath = Path.Combine(DataPath, "maps");
        private static readonly string ShopsPath = Path.Combine(DataPath, "shops");
        private static readonly string DataSub0Path = Path.Combine(ResourcePath, "data");
        private static readonly string DataSub1Path = Path.Combine(ResourcePath, "dataSub1");
        private static readonly string DataSub2Path = Path.Combine(ResourcePath, "dataSub2");
        private static readonly string MoversPropPath = Path.Combine(DataSub0Path, "propMover.txt");
        private static readonly string ItemsPropPath = Path.Combine(DataSub2Path, "propItem.txt");
        private static readonly string WorldScriptPath = Path.Combine(DataSub0Path, "World.inc");

        // Dictionary
        private static readonly IDictionary<string, int> Defines = new Dictionary<string, int>();
        private static readonly IDictionary<string, string> Texts = new Dictionary<string, string>();
        private static readonly IDictionary<int, MoverData> MoversData = new Dictionary<int, MoverData>();
        private static readonly IDictionary<int, ItemData> ItemsData = new Dictionary<int, ItemData>();
        private static readonly IDictionary<string, NpcData> NpcData = new Dictionary<string, NpcData>();
        private static readonly IDictionary<string, ShopData> ShopData = new Dictionary<string, ShopData>();
        private static readonly IDictionary<string, DialogData> DialogData = new Dictionary<string, DialogData>();

        // Managers
        public static readonly BehaviorManager<Game.Entities.IMonsterEntity> MonsterBehaviors = new BehaviorManager<Game.Entities.IMonsterEntity>(BehaviorType.Monster);
        public static readonly BehaviorManager<Game.Entities.INpcEntity> NpcBehaviors = new BehaviorManager<Game.Entities.INpcEntity>(BehaviorType.Npc);
        public static readonly BehaviorManager<Game.Entities.IPlayerEntity> PlayerBehaviors = new BehaviorManager<Game.Entities.IPlayerEntity>(BehaviorType.Player);

        // Logs messages
        private const string MsgUnableLoadMap = "Unable to load map '{0}'. Reason: {1}.";
        private const string MsgObjectIgnored = "{0} id '{1}' was ignored. Reason: {2}.";
        private const string MsgObjectOverrided = "{0} id '{1}' was overrided. Reason: {2}.";

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
            this.LoadShops();
            this.LoadDialogs();
            this.LoadNpc();
            this.LoadMaps();
            this.CleanUp();

            Logger.Info("Resources loaded in {0}ms.", Profiler.Stop("LoadResources").ElapsedMilliseconds);
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
                        var isIntValue = int.TryParse(define.Value.ToString(), out int intValue);

                        if (isIntValue && !Defines.ContainsKey(define.Key))
                            Defines.Add(define.Key, intValue);
                        else
                        {
                            Logger.Warn(MsgObjectIgnored, "Define", define.Key,
                                isIntValue ? "already declared" : $"'{define.Value}' is not a integer value");
                        }
                    }
                }
            }

            Logger.Info("-> {0} defines found.", Defines.Count);

            foreach (var textFilePath in textFiles)
            {
                using (var textFile = new TextFile(textFilePath))
                {
                    foreach (var text in textFile.Texts)
                    {
                        if (!Texts.ContainsKey(text.Key) && text.Value != null)
                            Texts.Add(text);
                        else
                        {
                            Logger.Warn(MsgObjectIgnored, "Text", text.Key,
                                (text.Value == null) ? "cannot get the value" : "already declared");
                        }
                    }
                }
            }

            Logger.Info("-> {0} texts found.", Texts.Count);
        }

        private void LoadMovers()
        {
            if (!File.Exists(MoversPropPath))
            {
                Logger.Warn("Unable to load movers. Reason: cannot find '{0}' file.", MoversPropPath);
                return;
            }

            using (var moversPropFile = new ResourceTableFile(MoversPropPath, 1, Defines, Texts))
            {
                var movers = moversPropFile.GetRecords<MoverData>();

                foreach (var mover in movers)
                {
                    if (MoversData.ContainsKey(mover.Id))
                    {
                        MoversData[mover.Id] = mover;
                        Logger.Warn(MsgObjectOverrided, "Mover", mover.Id, "already declared");
                    }
                    else
                        MoversData.Add(mover.Id, mover);
                }
            }

            Logger.Info("-> {0} movers loaded.", MoversData.Count);
        }

        private void LoadBehaviors()
        {
            MonsterBehaviors.Load();
            NpcBehaviors.Load();
            PlayerBehaviors.Load();

            Logger.Info("-> {0} behaviors loaded.", 
                MonsterBehaviors.Count + NpcBehaviors.Count + PlayerBehaviors.Count);
        }

        private void LoadItems()
        {
            if (!File.Exists(ItemsPropPath))
            {
                Logger.Warn("Unable to load items. Reason: cannot find '{0}' file.", ItemsPropPath);
                return;
            }

            using (var propItem = new ResourceTableFile(ItemsPropPath, 1, Defines, Texts))
            {
                var items = propItem.GetRecords<ItemData>();

                foreach (var item in items)
                {
                    if (ItemsData.ContainsKey(item.Id))
                    {
                        ItemsData[item.Id] = item;
                        Logger.Warn(MsgObjectOverrided, "Item", item.Id, "already declared");
                    } 
                    else
                        ItemsData.Add(item.Id, item);
                }
            }

            Logger.Info("-> {0} items loaded.", ItemsData.Count);
        }

        private void LoadShops()
        {
            if (!Directory.Exists(ShopsPath))
            {
                Logger.Warn("Unable to load shops. Reason: cannot find '{0}' directory.", ShopsPath);
                return;
            }

            string[] shopsFiles = Directory.GetFiles(ShopsPath);

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
                    {
                        if (ShopData.ContainsKey(shop.Name))
                            Logger.Debug(MsgObjectIgnored, "Shop", shop.Name, "already declared");
                        else
                            ShopData.Add(shop.Name, shop);
                    }
                        
                }
                else
                {
                    var shop = shopsParsed.ToObject<ShopData>();

                    if (ShopData.ContainsKey(shop.Name))
                        Logger.Debug(MsgObjectIgnored, "Shop", shop.Name, "already declared");
                    else
                        ShopData.Add(shop.Name, shop);
                }
            }

            Logger.Info("-> {0} shops loaded.", ShopData.Count);
        }

        private void LoadDialogs()
        {
            string currentDialogsPath = Path.Combine(DialogsPath, this.WorldConfiguration.Language);

            if (!Directory.Exists(currentDialogsPath))
            {
                Logger.Warn("Unable to load dialogs. Reason: cannot find '{0}' directory.", currentDialogsPath);
                return;
            }

            string[] dialogFiles = Directory.GetFiles(currentDialogsPath);

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
                    {
                        if (DialogData.ContainsKey(dialog.Name))
                            Logger.Debug(MsgObjectIgnored, "Dialog", dialog.Name, "already declared");
                        else
                            DialogData.Add(dialog.Name, dialog);
                    }
                }
                else
                {
                    var dialog = dialogsParsed.ToObject<DialogData>();

                    if (DialogData.ContainsKey(dialog.Name))
                        Logger.Debug(MsgObjectIgnored, "Dialog", dialog.Name, "already declared");
                    else
                        DialogData.Add(dialog.Name, dialog);
                }
            }

            Logger.Info("-> {0} dialogs loaded.", DialogData.Count);
        }

        private void LoadNpc()
        {
            IEnumerable<string> files = Directory.GetFiles(ResourcePath, "character*.inc", SearchOption.AllDirectories);

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

                        // We gets the npc name.
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

                        //TODO: implement other npc settings (image, music, actions...)
                        //      + constants for statement (like SetName)

                        // We gets shop and dialog  of this npc.
                        ShopData.TryGetValue(npcId, out ShopData npcShop);
                        DialogData.TryGetValue(npcId, out DialogData npcDialog);

                        var npc = new NpcData(npcId, npcName, npcShop, npcDialog);

                        if (NpcData.ContainsKey(npc.Id))
                        {
                            NpcData[npc.Id] = npc;
                            Logger.Warn(MsgObjectOverrided, "NPC", npc.Id, "already declared");
                        }
                        else
                            NpcData.Add(npc.Id, npc);
                    }
                }
            }

            Logger.Info("-> {0} NPCs loaded.", NpcData.Count);
        }

        private void LoadMaps()
        {
            IEnumerable<Type> systemTypes = this.LoadSystems();
            IReadOnlyDictionary<string, string> worldsPaths = this.LoadWorldScript();

            foreach (string mapDefineName in this.WorldConfiguration.Maps)
            {
                if (!worldsPaths.TryGetValue(mapDefineName, out string mapName))
                {
                    Logger.Warn(MsgUnableLoadMap, mapDefineName, $"map is not declared inside '{WorldScriptPath}' file");
                    continue;
                }

                if (!Defines.TryGetValue(mapDefineName, out int mapId))
                {
                    Logger.Warn(MsgUnableLoadMap, mapDefineName, $"map has no define id inside '{DataSub0Path}/defineWorld.h' file");
                    continue;
                }

                if (_maps.ContainsKey(mapId))
                {
                    Logger.Warn(MsgUnableLoadMap, mapDefineName, $"another map with id '{mapId}' already exist.");
                    continue;
                }

                IMapInstance map = MapInstance.Create(Path.Combine(MapsPath, mapName), mapName, mapId);

                foreach (Type type in systemTypes)
                    map.AddSystem(Activator.CreateInstance(type, map) as ISystem);

                _maps.Add(mapId, map);
                map.StartUpdateTask(100);
            }

            Logger.Info("-> {0} maps loaded.", _maps.Count);
        }

        private void CleanUp()
        {
            Defines.Clear();
            Texts.Clear();
            ShopData.Clear();
            GC.Collect();
        }

        private IEnumerable<Type> LoadSystems()
        {
            ChatSystem.Initialize();

            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.GetTypeInfo().GetCustomAttribute<SystemAttribute>() != null && typeof(ISystem).IsAssignableFrom(x));
        }

        private IReadOnlyDictionary<string, string> LoadWorldScript()
        {
            var worldsPaths = new Dictionary<string, string>();

            using (var textFile = new TextFile(WorldScriptPath))
            {
                foreach (var text in textFile.Texts)
                    worldsPaths.Add(text.Key, text.Value.Replace('"', ' ').Trim());
            }

            return worldsPaths;
        }
    }
}