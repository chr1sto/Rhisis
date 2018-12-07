using Newtonsoft.Json.Linq;
using NLog;
using Rhisis.Core.IO;
using Rhisis.Core.Resources;
using Rhisis.Core.Structures.Game;
using System;
using System.Collections.Generic;
using System.IO;

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
        private static readonly IDictionary<string, ShopData> ShopData = new Dictionary<string, ShopData>();

        // Logs messages
        private const string UnableLoadMapMessage = "Unable to load map '{0}'. Reason: {1}.";
        private const string ObjectIgnoredMessage = "{0} id '{1}' was ignored. Reason: {2}.";
        private const string ObjectOverridedMessage = "{0} id '{1}' was overrided. Reason: {2}.";

        private ILogger Logger;
        
        /// <summary>
        /// Loads the server's resources.
        /// </summary>
        private void LoadResources()
        {
            Logger.Info("Loading resources...");
            Profiler.Start("LoadResources");
            
            this.LoadShops();
            this.LoadJobs();
            this.CleanUp();

            Logger.Info("Resources loaded in {0}ms.", Profiler.Stop("LoadResources").ElapsedMilliseconds);
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

        private void CleanUp()
        {
            Defines.Clear();
            Texts.Clear();
            ShopData.Clear();
            GC.Collect();
        }
    }
}