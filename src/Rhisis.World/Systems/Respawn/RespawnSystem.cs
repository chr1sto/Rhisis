using Microsoft.Extensions.Logging;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Helpers;
using Rhisis.Core.IO;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Systems
{
    [Injectable]
    public sealed class RespawnSystem : IRespawnSystem
    {
        private readonly ILogger<RespawnSystem> _logger;

        /// <summary>
        /// Creates a new <see cref="RespawnSystem"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public RespawnSystem(ILogger<RespawnSystem> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public void Execute(IWorldEntity entity)
        {
            if (entity is IMonsterEntity monster && monster.IsDead)
            {
                if (monster.Object.Spawned && monster.Timers.DespawnTime < Time.TimeInSeconds())
                {
                    _logger.LogDebug($"Despawning {monster.Object.Name}...");
                    monster.Object.Spawned = false;
                    
                    if (!monster.Object.AbleRespawn)
                    {
                        monster.Timers.RespawnTime = Time.TimeInSeconds() + monster.Region.Time;
                    }
                }
                else if (!monster.Object.Spawned && monster.Timers.RespawnTime < Time.TimeInSeconds() && !monster.Object.AbleRespawn)
                {
                    _logger.LogDebug($"Respawning {monster.Object.Name}...");
                    ResetMonster(monster);
                }
            }

            if (entity is IItemEntity dropItem)
            {
                if (dropItem.Drop.HasOwner && dropItem.Drop.OwnershipTime <= Time.TimeInSeconds())
                {
                    ResetDropOwnership(dropItem);
                }

                if (dropItem.Drop.IsTemporary && dropItem.Drop.DespawnTime <= Time.TimeInSeconds())
                {
                    ResetDropOwnership(dropItem);
                    dropItem.Object.Spawned = false;
                    dropItem.Delete();
                }

                if (!dropItem.Drop.IsTemporary && !dropItem.Object.Spawned && dropItem.Drop.RespawnTime <= Time.TimeInSeconds())
                {
                    dropItem.Object.Position = dropItem.Region.GetRandomPosition();
                    dropItem.Object.Spawned = true;
                }
            }
        }

        /// <summary>
        /// Resets a monster.
        /// </summary>
        /// <param name="monster"></param>
        private void ResetMonster(IMonsterEntity monster)
        {
            monster.Timers.NextMoveTime = Time.TimeInSeconds() + RandomHelper.LongRandom(5, 15);
            monster.Object.Spawned = true;
            monster.Object.Position = monster.Region.GetRandomPosition();
            monster.Object.MovingFlags = ObjectState.OBJSTA_STAND;
            monster.Moves.DestinationPosition.Reset();
            monster.Moves.SpeedFactor = 1;
            monster.Battle.Reset();
            monster.Follow.Reset();
            monster.Attributes[DefineAttributes.HP] = monster.Data.AddHp;
            monster.Attributes[DefineAttributes.MP] = monster.Data.AddMp;
        }

        /// <summary>
        /// Resets a drop ownership.
        /// </summary>
        /// <param name="dropItem"></param>
        private void ResetDropOwnership(IItemEntity dropItem)
        {
            dropItem.Drop.Owner = null;
            dropItem.Drop.OwnershipTime = 0;
        }
    }
}
