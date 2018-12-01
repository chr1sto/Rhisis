using Newtonsoft.Json.Linq;
using NLog;
using Rhisis.Core.IO;
using Rhisis.Core.Resources;
using Rhisis.Core.Resources.Include;
using Rhisis.Core.Structures.Game;
using Rhisis.World.Game.Behaviors;
using Rhisis.World.Game.Core.Systems;
using Rhisis.World.Game.Maps;
using Rhisis.World.Systems.Chat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        private static readonly string JobPropPath = Path.Combine(DataSub1Path, "propJob.inc");

        // Dictionary
        private static readonly IDictionary<string, int> Defines = new Dictionary<string, int>();
        private static readonly IDictionary<string, string> Texts = new Dictionary<string, string>();
        private static readonly IDictionary<int, JobData> JobsData = new Dictionary<int, JobData>();
        private static readonly IDictionary<string, NpcData> NpcData = new Dictionary<string, NpcData>();
        private static readonly IDictionary<string, ShopData> ShopData = new Dictionary<string, ShopData>();
        private static readonly IDictionary<string, DialogData> DialogData = new Dictionary<string, DialogData>();

        // Managers
        public static readonly BehaviorManager<Game.Entities.IMonsterEntity> MonsterBehaviors = new BehaviorManager<Game.Entities.IMonsterEntity>(BehaviorType.Monster);
        public static readonly BehaviorManager<Game.Entities.INpcEntity> NpcBehaviors = new BehaviorManager<Game.Entities.INpcEntity>(BehaviorType.Npc);
        public static readonly BehaviorManager<Game.Entities.IPlayerEntity> PlayerBehaviors = new BehaviorManager<Game.Entities.IPlayerEntity>(BehaviorType.Player);

        // Logs messages
        private const string UnableLoadMapMessage = "Unable to load map '{0}'. Reason: {1}.";
        private const string ObjectIgnoredMessage = "{0} id '{1}' was ignored. Reason: {2}.";
        private const string ObjectOverridedMessage = "{0} id '{1}' was overrided. Reason: {2}.";

        private ILogger Logger;

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
            
            this.LoadBehaviors();
            this.LoadShops();
            this.LoadDialogs();
            this.LoadNpc();
            this.LoadJobs();
            this.CleanUp();

            Logger.Info("Resources loaded in {0}ms.", Profiler.Stop("LoadResources").ElapsedMilliseconds);
        }

        private void LoadBehaviors()
        {
            MonsterBehaviors.Load();
            NpcBehaviors.Load();
            PlayerBehaviors.Load();

            Logger.Info("-> {0} behaviors loaded.", 
                MonsterBehaviors.Count + NpcBehaviors.Count + PlayerBehaviors.Count);
        }

        private void LoadJobs()
        {
            if (!File.Exists(JobPropPath))
            {
                Logger.Warn($"Unable to load job properties. Reason: cannot find '{JobPropPath}' file.");
                return;
            }

            using (var propJob = new ResourceTableFile(JobPropPath, -1, new char[] { '\t', ' ', '\r' }, Defines, null))
            {
                var jobs = propJob.GetRecords<JobData>();

                foreach (var job in jobs)
                {
                    if (JobsData.ContainsKey(job.Id))
                    {
                        JobsData[job.Id] = job;
                        Logger.Warn(ObjectOverridedMessage, "JobData", job.Id, "already delcared");
                    }
                    else
                        JobsData.Add(job.Id, job);
                }
            }

            Logger.Info($"-> {JobsData.Count} jobs data loaded.");
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
                            Logger.Debug(ObjectIgnoredMessage, "Shop", shop.Name, "already declared");
                        else
                            ShopData.Add(shop.Name, shop);
                    }
                        
                }
                else
                {
                    var shop = shopsParsed.ToObject<ShopData>();

                    if (ShopData.ContainsKey(shop.Name))
                        Logger.Debug(ObjectIgnoredMessage, "Shop", shop.Name, "already declared");
                    else
                        ShopData.Add(shop.Name, shop);
                }
            }

            Logger.Info("-> {0} shops loaded.", ShopData.Count);
        }

        private void LoadDialogs()
        {
            string currentDialogsPath = Path.Combine(DialogsPath, /*this.WorldConfiguration.Language*/ "");

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
                            Logger.Debug(ObjectIgnoredMessage, "Dialog", dialog.Name, "already declared");
                        else
                            DialogData.Add(dialog.Name, dialog);
                    }
                }
                else
                {
                    var dialog = dialogsParsed.ToObject<DialogData>();

                    if (DialogData.ContainsKey(dialog.Name))
                        Logger.Debug(ObjectIgnoredMessage, "Dialog", dialog.Name, "already declared");
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
                            Logger.Warn(ObjectOverridedMessage, "NPC", npc.Id, "already declared");
                        }
                        else
                            NpcData.Add(npc.Id, npc);
                    }
                }
            }

            Logger.Info("-> {0} NPCs loaded.", NpcData.Count);
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