using Rhisis.Core.IO;
using Rhisis.Core.Resources;
using Rhisis.Core.Structures.Game;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rhisis.Cluster
{
    public partial class ClusterServer
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
        private static readonly string JobPropPath = Path.Combine(DataSub1Path, "propJob.inc");

        private static readonly IDictionary<string, int> Defines = new Dictionary<string, int>();
        private static readonly IDictionary<int, JobData> JobData = new Dictionary<int, JobData>();

        // Logs messages
        private const string UnableLoadMapMessage = "Unable to load map '{0}'. Reason: {1}.";
        private const string ObjectIgnoredMessage = "{0} id '{1}' was ignored. Reason: {2}.";
        private const string ObjectOverridedMessage = "{0} id '{1}' was overrided. Reason: {2}.";

        public static readonly IReadOnlyDictionary<int, JobData> JobsData = JobData as IReadOnlyDictionary<int, JobData>;

        private void LoadResources()
        {
            Logger.Info("Loading resources...");
            Profiler.Start("LoadResources");

            this.LoadDefines();
            this.LoadJobs();

            Logger.Info("Resources loaded in {0}ms.", Profiler.Stop("LoadResources").ElapsedMilliseconds);
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
                        var isIntValue = int.TryParse(define.Value.ToString(), out int intValue);

                        if (isIntValue && !Defines.ContainsKey(define.Key))
                            Defines.Add(define.Key, intValue);
                        else
                        {
                            Logger.Warn(ObjectIgnoredMessage, "Define", define.Key,
                                isIntValue ? "already declared" : $"'{define.Value}' is not a integer value");
                        }
                    }
                }
            }
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
    }
}
