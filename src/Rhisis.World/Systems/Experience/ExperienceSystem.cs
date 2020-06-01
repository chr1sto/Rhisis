using Rhisis.Core.Common.Formulas;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Resources;
using Rhisis.Core.Structures.Game;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using System.Collections.Generic;

namespace Rhisis.World.Systems.Experience
{
    [Injectable]
    public sealed class ExperienceSystem : IExperienceSystem
    {
        private static readonly IReadOnlyDictionary<DefineJob.JobType, int> _experienceLevelLimits = new Dictionary<DefineJob.JobType, int>()
        {
            { DefineJob.JobType.JTYPE_BASE, (int)DefineJob.JobMax.MAX_JOB_LEVEL },
            { DefineJob.JobType.JTYPE_EXPERT, (int)DefineJob.JobMax.MAX_JOB_LEVEL + (int)DefineJob.JobMax.MAX_EXP_LEVEL },
            { DefineJob.JobType.JTYPE_PRO, (int)DefineJob.JobMax.MAX_LEVEL },
            { DefineJob.JobType.JTYPE_MASTER, (int)DefineJob.JobMax.MAX_LEVEL },
            { DefineJob.JobType.JTYPE_HERO, (int)DefineJob.JobMax.MAX_LEGEND_LEVEL }
        };

        private readonly IGameResources _gameResources;
        private readonly IMoverPacketFactory _moverPacketFactory;
        private readonly IPlayerPacketFactory _playerPacketFactory;

        /// <summary>
        /// Creates a new <see cref="ExperienceSystem"/> instance.
        /// </summary>
        /// <param name="gameResources">Game resources.</param>
        /// <param name="playerPacketFactory">Player packet factory.</param>
        public ExperienceSystem(IGameResources gameResources, IMoverPacketFactory moverPacketFactory, IPlayerPacketFactory playerPacketFactory)
        {
            _gameResources = gameResources;
            _moverPacketFactory = moverPacketFactory;
            _playerPacketFactory = playerPacketFactory;
        }

        /// <inheritdoc />
        public void GiveExeperience(IPlayerEntity player, long experience)
        {
            if (player.PlayerData.Mode.HasFlag(ModeType.MODE_EXPUP_STOP))
            {
                return;
            }
            
            var exp = CalculateExtraExperience(player, experience);

            // TODO: experience to party

            if (GiveExperienceToPlayer(player, exp))
            {
                _moverPacketFactory.SendUpdateAttributes(player, DefineAttributes.HP, player.Attributes[DefineAttributes.HP]);
                _moverPacketFactory.SendUpdateAttributes(player, DefineAttributes.MP, player.Attributes[DefineAttributes.MP]);
                _moverPacketFactory.SendUpdateAttributes(player, DefineAttributes.FP, player.Attributes[DefineAttributes.FP]);
                _playerPacketFactory.SendPlayerSetLevel(player, player.Object.Level);
                _playerPacketFactory.SendPlayerStatsPoints(player);
            }

            _playerPacketFactory.SendPlayerExperience(player);
            // TODO: send packet to friends, messenger, guild, couple, party, etc...
        }

        /// <summary>
        /// Give experience to a player and returns a boolean value that indicates if the player has level up.
        /// </summary>
        /// <param name="player">Player entity.</param>
        /// <param name="experience">Experience to give.</param>
        /// <returns>True if the player has level up; false otherwise.</returns>
        private bool GiveExperienceToPlayer(IPlayerEntity player, long experience)
        {
            if (HasReachedMaxLevel(player))
            {
                player.PlayerData.Experience = 0;

                return false;
            }

            var nextLevel = player.Object.Level + 1;
            CharacterExpTableData nextLevelExpTable = _gameResources.ExpTables.GetCharacterExp(nextLevel);
            player.PlayerData.Experience += experience;

            if (player.PlayerData.Experience >= nextLevelExpTable.Exp) // Level up
            {
                var remainingExp = player.PlayerData.Experience - nextLevelExpTable.Exp;

                ProcessLevelUp(player, (ushort)nextLevelExpTable.Gp);

                if (remainingExp > 0)
                {
                    GiveExperienceToPlayer(player, remainingExp); // Multiple level up
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Calculates extra experience with scrolls, events, skill bonus, etc...
        /// </summary>
        /// <param name="player">Player entity.</param>
        /// <param name="experience">Current experience.</param>
        /// <returns>Base experience with extra experience.</returns>
        private long CalculateExtraExperience(IPlayerEntity player, long experience)
        {
            // TODO: add exp scrolls logic here

            return experience;
        }

        /// <summary>
        /// Process the level up logic and reward attribution.
        /// </summary>
        /// <param name="player">Player entity.</param>
        /// <param name="statPoints">Statistics points.</param>
        private void ProcessLevelUp(IPlayerEntity player, ushort statPoints)
        {
            player.Object.Level += 1;

            if (player.Object.Level != player.PlayerData.DeathLevel)
            {
                player.Statistics.SkillPoints += (ushort)((player.Object.Level - 1) / 20 + 2);
                player.Statistics.StatPoints += statPoints;
            }

            var strength = player.Attributes[DefineAttributes.STR];
            var stamina = player.Attributes[DefineAttributes.STA];
            var dexterity = player.Attributes[DefineAttributes.DEX];
            var intelligence = player.Attributes[DefineAttributes.INT];

            player.PlayerData.Experience = 0;
            player.Attributes[DefineAttributes.HP] = HealthFormulas.GetMaxOriginHp(player.Object.Level, stamina, player.PlayerData.JobData.MaxHpFactor);
            player.Attributes[DefineAttributes.MP] = HealthFormulas.GetMaxOriginMp(player.Object.Level, intelligence, player.PlayerData.JobData.MaxMpFactor, true);
            player.Attributes[DefineAttributes.FP] = HealthFormulas.GetMaxOriginFp(player.Object.Level, stamina, dexterity, strength, player.PlayerData.JobData.MaxFpFactor, true);
        }

        /// <summary>
        /// Checks if the player has reached the maximum level for its job.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <returns>True if the player has reached the maximum level for its job; false otherwise.</returns>
        private bool HasReachedMaxLevel(IPlayerEntity player)
        {
            if (!_experienceLevelLimits.TryGetValue(player.PlayerData.JobData.Type, out int limitLevel))
            {
                return false;
            }

            return player.Object.Level >= limitLevel;
        }
    }
}
