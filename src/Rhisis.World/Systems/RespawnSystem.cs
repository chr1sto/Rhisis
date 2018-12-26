using Microsoft.Extensions.Logging;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.IO;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Systems;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Systems
{
    [System]
    public sealed class RespawnSystem : ISystem
    {
        private static readonly ILogger<RespawnSystem> Logger = DependencyContainer.Instance.Resolve<ILogger<RespawnSystem>>();

        public WorldEntityType Type => WorldEntityType.Monster | WorldEntityType.Drop;

        public void Execute(IEntity entity, SystemEventArgs args)
        {
            if (entity is IMonsterEntity monster && monster.Health.IsDead)
            {
                if (monster.Object.Spawned && monster.Timers.DespawnTime < Time.TimeInSeconds())
                {
                    Logger.LogDebug($"Despawning {monster.Object.Name}...");
                    monster.Object.Spawned = false;
                    monster.Timers.RespawnTime = Time.TimeInSeconds() + monster.Region.Time;
                }
                else if (!monster.Object.Spawned && monster.Timers.RespawnTime < Time.TimeInSeconds())
                {
                    Logger.LogDebug($"Respawning {monster.Object.Name}...");
                    this.ResetMonster(monster);
                    monster.Object.Spawned = true;
                }
            }
        }

        private void ResetMonster(IMonsterEntity monster)
        {
            monster.Health.Hp = monster.Data.AddHp;
            monster.Health.Mp = monster.Data.AddMp;
            monster.Object.Position = monster.Region.GetRandomPosition();
            monster.MovableComponent.DestinationPosition = monster.Object.Position.Clone();
        }
    }
}
