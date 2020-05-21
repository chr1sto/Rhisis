using Microsoft.Extensions.Logging;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Helpers;
using Rhisis.World.Packets;

namespace Rhisis.World.Systems.Statistics
{
    [Injectable]
    public sealed class StatisticsSystem : IStatisticsSystem
    {
        private readonly ILogger<StatisticsSystem> _logger;
        private readonly IPlayerPacketFactory _playerPacketFactory;

        /// <summary>
        /// Creates a new <see cref="StatisticsSystem"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public StatisticsSystem(ILogger<StatisticsSystem> logger, IPlayerPacketFactory playerPacketFactory)
        {
            _logger = logger;
            _playerPacketFactory = playerPacketFactory;
        }

        public void UpdateStatistics(IPlayerEntity player, ushort strength, ushort stamina, ushort dexterity, ushort intelligence)
        {
            _logger.LogDebug("Modify sttus");

            var total = strength + stamina + dexterity + intelligence;

            var statsPoints = player.Statistics.StatPoints;
            if (statsPoints <= 0 || total > statsPoints)
            {
                _logger.LogError("No statspoints available, but trying to upgrade {0}.", player.Object.Name);
                return;
            }

            if (strength > statsPoints || stamina > statsPoints ||
                dexterity > statsPoints || intelligence > statsPoints || total <= 0 ||
                total > ushort.MaxValue)
            {
                _logger.LogError("Invalid upgrade request due to bad total calculation (trying to dupe) {0}.",
                    player.Object.Name);
                return;
            }

            player.Attributes.IncreaseAttribute(DefineAttributes.STR, strength);
            player.Attributes.IncreaseAttribute(DefineAttributes.STA, stamina);
            player.Attributes.IncreaseAttribute(DefineAttributes.DEX, dexterity);
            player.Attributes.IncreaseAttribute(DefineAttributes.INT, intelligence);
            player.Statistics.StatPoints -= (ushort)total;

            _playerPacketFactory.SendPlayerUpdateState(player);
        }

        public void Restat(IPlayerEntity player)
        {
            const int DefaultAttributePoints = 15;

            player.Attributes[DefineAttributes.STR] = DefaultAttributePoints;
            player.Attributes[DefineAttributes.STA] = DefaultAttributePoints;
            player.Attributes[DefineAttributes.DEX] = DefaultAttributePoints;
            player.Attributes[DefineAttributes.INT] = DefaultAttributePoints;
            player.Attributes[DefineAttributes.HP] = PlayerHelper.GetMaxHP(player);
            player.Attributes[DefineAttributes.MP] = PlayerHelper.GetMaxMP(player);
            player.Attributes[DefineAttributes.FP] = PlayerHelper.GetMaxFP(player);

            player.Statistics.StatPoints = (ushort)((player.Object.Level - 1) * 2);

            _playerPacketFactory.SendPlayerUpdateState(player);
        }
    }
}
