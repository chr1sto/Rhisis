using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
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

            if (!File.Exists(propJobFile))
            {
                _logger.LogWarning($"Unable to load job properties. Reason: cannot find '{propJobFile}' file.");
                return;
            }

            var jobData = new ConcurrentDictionary<int, JobData>();
            using (var propJob = new ResourceTableFile(propJobFile, -1, new [] { '\t', ' ', '\r' }, _defines, null))
            {
                var jobs = propJob.GetRecords<JobData>();

                foreach (var job in jobs)
                {
                    if (jobData.ContainsKey(job.Id))
                    {
                        jobData[job.Id] = job;
                        _logger.LogWarning(GameResourcesConstants.Errors.ObjectOverridedMessage, "JobData", job.Id, "already delcared");
                    }
                    else
                        jobData.TryAdd(job.Id, job);
                }
            }

            _cache.Set(GameResourcesConstants.Jobs, jobData);
            _logger.LogInformation($"-> {jobData.Count} jobs data loaded.");
        }
    }
}
