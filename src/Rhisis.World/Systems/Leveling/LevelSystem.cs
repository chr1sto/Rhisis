using Microsoft.Extensions.Logging;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Resources;
using Rhisis.Core.Structures.Game;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Systems;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Leveling.EventArgs;
using System;

namespace Rhisis.World.Systems.Leveling
{
    /// <summary>
    /// Leveling system.
    /// </summary>
    [System(SystemType.Notifiable)]
    public sealed class LevelSystem : ISystem
    {
        private readonly ILogger<LevelSystem> _logger = DependencyContainer.Instance.Resolve<ILogger<LevelSystem>>();

        /// <inheritdoc />
        public WorldEntityType Type => WorldEntityType.Player;

        /// <inheritdoc />
        public void Execute(IEntity entity, SystemEventArgs args)
        {
            if (!(entity is IPlayerEntity player))
            {
                this._logger.LogError($"Cannot execute Level System. {entity.Object.Name} is not a player.");
                return;
            }

            if (!args.CheckArguments())
            {
                this._logger.LogError($"Cannot execute Level System action: {args.GetType()} due to invalid arguments.");
                return;
            }

            switch (args)
            {
                case ExperienceEventArgs e:
                    this.GiveExperience(player, e);
                    break;
                default:
                    this._logger.LogWarning("Unknown level system action type: {0} for player {1}", args.GetType(), entity.Object.Name);
                    break;
            }
        }

        private void GiveExperience(IPlayerEntity player, ExperienceEventArgs e)
        {
            long experience = this.CalculateExtraExperience(player, e.Experience);

            // TODO: experience to party

            if (this.GiveExperienceToPlayer(player, experience))
            {
                WorldPacketFactory.SendPlayerSetLevel(player, player.Object.Level);
                WorldPacketFactory.SendPlayerStatsPoints(player);
            }

            WorldPacketFactory.SendPlayerExperience(player);
        }

        private bool GiveExperienceToPlayer(IPlayerEntity player, long experience)
        {
            int nextLevel = player.Object.Level + 1;
            CharacterExpTableData nextLevelExpTable = GameResources.Instance.ExpTables.GetCharacterExp(nextLevel);
            player.PlayerData.Experience += experience;

            if (player.PlayerData.Experience >= nextLevelExpTable.Exp) // Level up
            {
                long remainingExp = player.PlayerData.Experience - nextLevelExpTable.Exp;

                player.Object.Level += 1;
                player.Statistics.SkillPoints += (ushort)(((player.Object.Level - 1) / 20) + 2);
                player.Statistics.StatPoints += (ushort)nextLevelExpTable.Gp;
                player.PlayerData.Experience = 0;

                if (remainingExp > 0)
                    this.GiveExperienceToPlayer(player, remainingExp); // Multiple level up

                return true;
            }

            return false;
        }

        private long CalculateExtraExperience(IPlayerEntity player, long experience)
        {
            // TODO: add exp scrolls logic here

            return experience;
        }
    }
}
