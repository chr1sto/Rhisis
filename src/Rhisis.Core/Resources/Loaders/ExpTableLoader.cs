using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Extensions;
using Rhisis.Core.Resources.Include;
using Rhisis.Core.Structures.Game;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rhisis.Core.Resources.Loaders
{
    public class ExpTableLoader : IGameResourceLoader
    {
        private readonly ILogger<ExpTableLoader> _logger;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Creates a new <see cref="ExpTableLoader"/> instance.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="cache">Memory cache.</param>
        public ExpTableLoader(ILogger<ExpTableLoader> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        /// <inheritdoc />
        public void Load()
        {
            string expTablePath = GameResourcesConstants.Paths.ExpTablePath;

            if (!File.Exists(expTablePath))
            {
                _logger.LogWarning("Unable to load exp table. Reason: Cannot find '{0}' file.", expTablePath);
                return;
            }

            using (var expTableFile = new IncludeFile(expTablePath, @"([(){}=,;\n\r\t ])"))
            {
                var dropLuckBlock = expTableFile.GetBlock("expDropLuck");
                if (dropLuckBlock is null)
                {
                    _logger.LogWarning("Unable to load exp table. Reason: Cannot find drop luck data.");
                    return;
                }
                
                var expCharacterBlock = expTableFile.GetBlock("expCharacter");
                if (expCharacterBlock is null)
                {
                    _logger.LogWarning("Unable to load exp table. Reason: Cannot find character experience data.");
                    return;
                }

                IEnumerable<long[]> dropLuck = LoadDropLuck(dropLuckBlock);
                IReadOnlyDictionary<int, CharacterExpTableData> characterExperience = LoadCharacterExperience(expCharacterBlock);
                var expTableData = new ExpTableData(dropLuck, characterExperience);
                _cache.Set(GameResourcesConstants.ExpTables, expTableData);
            }

            _logger.LogInformation("-> Experience tables loaded.");
        }

        /// <summary>
        /// Load a collection of item drop luck from a "expDropLuck" block instruction.
        /// </summary>
        /// <param name="dropLuckBlock"></param>
        private IEnumerable<long[]> LoadDropLuck(Block dropLuckBlock) 
            => dropLuckBlock.UnknownStatements.Select(x => long.Parse(x)).GroupBy(11).Select(x => x.ToArray());

        /// <summary>
        /// Loads the character experience table.
        /// </summary>
        /// <param name="expTableBlock"></param>
        private IReadOnlyDictionary<int, CharacterExpTableData> LoadCharacterExperience(Block expTableBlock)
        {
            return expTableBlock.UnknownStatements
                .Select((value, index) => new { Value = value, Index = index })
                .GroupBy(item => item.Index / 4, item => item.Value)
                .Select(x =>
                {
                    return new CharacterExpTableData(x.Key,
                        long.Parse(x.ElementAt(0)),
                        long.Parse(x.ElementAt(1)),
                        long.Parse(x.ElementAt(2)),
                        long.Parse(x.ElementAt(3)));
                }).ToDictionary(x => x.Level);
        }
    }
}
