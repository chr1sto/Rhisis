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

        /// <summary>
        /// Gets the item drop luck data.
        /// </summary>
        public IEnumerable<long[]> ExpDropLuck { get; private set; }

        /// <summary>
        /// Gets the Character experience table data.
        /// </summary>
        public IDictionary<int, CharacterExpTableData> CharacterExpTable { get; private set; }

        /// <summary>
        /// Creates a new <see cref="ExpTableLoader"/> instance.
        /// </summary>
        /// <param name="logger">Logger</param>
        public ExpTableLoader(ILogger<ExpTableLoader> logger)
        {
            this._logger = logger;
            this.CharacterExpTable = new Dictionary<int, CharacterExpTableData>();
        }

        /// <inheritdoc />
        public void Load()
        {
            if (!File.Exists(GameResources.ExpTablePath))
            {
                this._logger.LogWarning("Unable to load exp table. Reason: cannot find '{0}' file.", GameResources.ExpTablePath);
                return;
            }

            using (var expTableFile = new IncludeFile(GameResources.ExpTablePath, @"([(){}=,;\n\r\t ])"))
            {
                this.LoadDropLuck(expTableFile.GetBlock("expDropLuck"));
                this.LoadCharacterExperience(expTableFile.GetBlock("expCharacter"));
            }

            this._logger.LogInformation("-> Experience tables loaded.");
        }

        /// <summary>
        /// Load a collection of item drop luck from a "expDropLuck" block instruction.
        /// </summary>
        /// <param name="dropLuckBlock"></param>
        private void LoadDropLuck(Block dropLuckBlock) 
            => this.ExpDropLuck = dropLuckBlock.UnknownStatements.Select(x => long.Parse(x)).GroupBy(11).Select(x => x.ToArray());

        /// <summary>
        /// Loads the character experience table.
        /// </summary>
        /// <param name="expTableBlock"></param>
        private void LoadCharacterExperience(Block expTableBlock)
        {
            this.CharacterExpTable = expTableBlock.UnknownStatements
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

        /// <summary>
        /// Gets a drop luck by level and refine.
        /// </summary>
        /// <param name="level">Level</param>
        /// <param name="refine">Refine</param>
        /// <returns></returns>
        public long GetDropLuck(int level, int refine) => this.ExpDropLuck.ElementAt(level - 1).ElementAt(refine);

        /// <summary>
        /// Gets a character experience information based on a level.
        /// </summary>
        /// <param name="level">Character level.</param>
        /// <returns><see cref="CharacterExpTableData"/> matching the level.</returns>
        public CharacterExpTableData GetCharacterExp(int level) => this.CharacterExpTable.TryGetValue(level, out CharacterExpTableData value) ? value : default;

        /// <inheritdoc />
        public void Dispose()
        {
            this.ExpDropLuck = null;
            this.CharacterExpTable.Clear();
        }
    }
}
