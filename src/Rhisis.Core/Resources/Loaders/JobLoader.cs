using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Rhisis.Core.Data;
using Rhisis.Core.Structures.Game;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace Rhisis.Core.Resources.Loaders
{
    public sealed class JobLoader : IGameResourceLoader
    {
        private readonly ILogger<JobLoader> _logger;
        private readonly IMemoryCache _cache;
        private readonly IDictionary<string, int> _defines;

        /// <summary>
        /// Creates a new <see cref="JobLoader"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="cache">Application memory cache.</param>
        public JobLoader(ILogger<JobLoader> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
            _defines = _cache.Get<IDictionary<string, int>>(GameResourcesConstants.Defines);
        }

        /// <inheritdoc />
        public void Load()
        {
            string propJobFile = GameResourcesConstants.Paths.JobPropPath;
            string jobsDefinitionFile = GameResourcesConstants.Paths.JobsDefinitionsPath;

            if (!File.Exists(propJobFile))
            {
                _logger.LogWarning($"Unable to load job properties. Reason: cannot find '{propJobFile}' file.");
                return;
            }

            if (!File.Exists(jobsDefinitionFile))
            {
                _logger.LogWarning($"Unable to load job definitions. Reason: cannot find '{jobsDefinitionFile}' file.");
                return;
            }

            string jobDefinitionFileContent = File.ReadAllText(jobsDefinitionFile);
            var jobDefinitions = JsonConvert.DeserializeObject<Dictionary<DefineJob.Job, JobDefinitionData>>(jobDefinitionFileContent);
            var jobData = new ConcurrentDictionary<DefineJob.Job, JobData>();

            using (var propJob = new ResourceTableFile(propJobFile, -1, new [] { '\t', ' ', '\r' }, _defines, null))
            {
                IEnumerable<JobData> jobs = propJob.GetRecords<JobData>();

                foreach (JobData job in jobs)
                {
                    if (!jobData.TryAdd(job.Id, job))
                    {
                        _logger.LogWarning(GameResourcesConstants.Errors.ObjectOverridedMessage, "JobData", job.Id, "already delcared");
                    }
                }

                foreach (JobData job in jobData.Values)
                {
                    if (jobDefinitions.TryGetValue(job.Id, out JobDefinitionData jobDefinition))
                    {
                        job.Parent = jobDefinition.Parent.HasValue ? jobData[jobDefinition.Parent.Value] : null;
                        job.Type = jobDefinition.Type;
                    }
                    else
                    {
                        _logger.LogWarning($"Cannot find job '{job.Id}' definition.");
                    }
                }
            }

            _cache.Set(GameResourcesConstants.Jobs, jobData);
            _logger.LogInformation($"-> {jobData.Count} jobs data loaded.");
        }
    }
}
