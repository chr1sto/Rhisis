using Microsoft.Extensions.Logging;
using Rhisis.Core.Helpers;
using Rhisis.Core.Structures.Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rhisis.Core.Resources.Loaders
{
    /// <summary>
    /// Loads all server's penality data.
    /// </summary>
    public sealed class PenalityLoader : IGameResourceLoader
    {
        private readonly ILogger<PenalityLoader> _logger;
        
        /// <summary>
        /// Gets the death penality data.
        /// </summary>
        public DeathPenalityData DeathPenality { get; private set; }

        /// <summary>
        /// Creates a new <see cref="PenalityLoader"/> instance.
        /// </summary>
        /// <param name="logger"></param>
        public PenalityLoader(ILogger<PenalityLoader> logger)
        {
            this._logger = logger;
        }

        /// <inheritdoc />
        public void Load()
        {
            if (!File.Exists(GameResources.DeathPenalityPath))
            {
                this._logger.LogWarning("Unable to load death penality. Reason: cannot find '{0}' file.", GameResources.DeathPenalityPath);
                return;
            }

            this.DeathPenality = ConfigurationHelper.Load<DeathPenalityData>(GameResources.DeathPenalityPath);

            if (this.DeathPenality == null)
            {
                this._logger.LogError(GameResources.UnableLoadMessage, "death penality", "Json loading error.");
                return;
            }

            this.DeathPenality.RevivalPenality = this.DeathPenality.RevivalPenality.OrderBy(x => x.Level);
            this.DeathPenality.DecExpPenality = this.DeathPenality.DecExpPenality.OrderBy(x => x.Level);
            this.DeathPenality.LevelDownPenality = this.DeathPenality.LevelDownPenality.OrderBy(x => x.Level);

            this._logger.LogInformation("-> Penalities loaded.");
        }

        /// <summary>
        /// Gets a revival penality by a level.
        /// </summary>
        /// <param name="level">Level.</param>
        /// <returns>Revival penality expressed as a percentage.</returns>
        public decimal GetRevivalPenality(int level) => this.GetPenality(this.DeathPenality.RevivalPenality, level).Value;

        /// <summary>
        /// Gets the experience penality by a level.
        /// </summary>
        /// <param name="level">Level.</param>
        /// <returns>Experience penality expressed as a percentage.</returns>
        public decimal GetDecExpPenality(int level) => this.GetPenality(this.DeathPenality.DecExpPenality, level).Value;

        /// <summary>
        /// Gets the level down penality by a level.
        /// </summary>
        /// <param name="level">Level</param>
        /// <returns>Boolean value that indicates if level should go down.</returns>
        public bool GetLevelDownPenality(int level) 
            => Convert.ToBoolean(this.GetPenality(this.DeathPenality.LevelDownPenality, level).Value);

        /// <summary>
        /// Gets a <see cref="PenalityData"/> from a collection of penalities and a level.
        /// </summary>
        /// <param name="penalityData">Penality data colletion.</param>
        /// <param name="level">Level</param>
        /// <returns></returns>
        private PenalityData GetPenality(IEnumerable<PenalityData> penalityData, int level) 
            => penalityData.FirstOrDefault(x => level <= x.Level) ?? penalityData.LastOrDefault();

        /// <inheritdoc />
        public void Dispose()
        {
            this.DeathPenality = null;
        }
    }
}
