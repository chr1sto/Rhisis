using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Structures.Game;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rhisis.Core.Resources.Loaders
{
    /// <summary>
    /// Provides a mechanism to load the game skills and skill levels.
    /// </summary>
    public sealed class SkillLoader : IGameResourceLoader
    {
        private readonly ILogger<SkillLoader> _logger;
        private readonly IMemoryCache _cache;
        private readonly IDictionary<string, int> _defines;
        private readonly IDictionary<string, string> _texts;
        
        /// <summary>
        /// Creates a new <see cref="SkillLoader"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="cache">Memory cache.</param>
        public SkillLoader(ILogger<SkillLoader> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
            _defines = _cache.Get<IDictionary<string, int>>(GameResourcesConstants.Defines);
            _texts = _cache.Get<IDictionary<string, string>>(GameResourcesConstants.Texts);
        }

        /// <inheritdoc />
        public void Load()
        {
            string propSkillPath = GameResourcesConstants.Paths.SkillPropPath;
            string propSkillAddPath = GameResourcesConstants.Paths.SkillPropAddPath;

            if (!File.Exists(propSkillPath))
            {
                _logger.LogWarning("Unable to load skills. Reason: cannot find '{0}' file.", propSkillPath);
                return;
            }

            if (!File.Exists(propSkillAddPath))
            {
                _logger.LogWarning("Unable to load skills. Reason: cannot find '{0}' file.", propSkillAddPath);
                return;
            }

            using var propSkill = new ResourceTableFile(GameResourcesConstants.Paths.SkillPropPath, 1, _defines, _texts);
            using var propSkillAdd = new ResourceTableFile(propSkillAddPath, 1, new[] { ',' }, _defines, _texts);

            var skillsData = new ConcurrentDictionary<int, SkillData>();
            var skills = propSkill.GetRecords<SkillData>();
            var skillLevelsData = propSkillAdd.GetRecords<SkillLevelData>().GroupBy(x => x.SkillId).ToDictionary(x => x.Key, x => x.AsEnumerable());

            foreach (SkillData skillData in skills)
            {
                if (skillLevelsData.TryGetValue(skillData.Id, out IEnumerable<SkillLevelData> skillLevels))
                {
                    skillData.SkillLevels = skillLevels.ToDictionary(x => x.Level, x => x);

                    foreach (SkillLevelData skillLevel in skillData.SkillLevels.Values)
                    {
                        if (skillLevel.CooldownTime <= 0)
                        {
                            skillLevel.CooldownTime = skillData.SkillReadyTime;
                        }
                    }

                    if (!skillsData.TryAdd(skillData.Id, skillData))
                    {
                        _logger.LogWarning($"Cannot add skill '{skillData.Name}'. Reason: Already exist.");
                    }
                }
            }

            _cache.Set(GameResourcesConstants.Skills, skillsData);
            _logger.LogInformation("-> {0} skills loaded.", skillsData.Count);
        }
    }
}
