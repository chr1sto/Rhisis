using NLog;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Systems;
using Rhisis.World.Game.Entities;
using Rhisis.World.Systems.Interaction.EventArgs;

namespace Rhisis.World.Systems.Interaction
{
    [System(SystemType.Notifiable)]
    public class InteractionSystem : ISystem
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <inheritdoc />
        public WorldEntityType Type => WorldEntityType.Player;

        /// <summary>
        /// Executes the <see cref="InteractionSystem"/> logic.
        /// </summary>
        /// <param name="entity">Current entity</param>
        public void Execute(IEntity entity, SystemEventArgs e)
        {
            if (!(entity is IPlayerEntity playerEntity))
                return;

            if (!e.CheckArguments())
            {
                Logger.Error("Cannot execute interaction action: {0} due to invalid arguments.", e.GetType());
                return;
            }

            switch(e)
            {
                case SetTargetEventArgs setTargetEventArgs:
                    this.SetTarget(playerEntity, setTargetEventArgs);
                    break;
                default:
                    Logger.Warn("Unknown interaction action type: {0} for player {1}", e.GetType(), entity.Object.Name);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="e"></param>
        private void SetTarget(IPlayerEntity player, SetTargetEventArgs e)
        {
            var targetEntity = player.FindEntity<IEntity>(e.TargetId);
            if (targetEntity == null)
                return;

            if (e.Clear == 2)
            {
                player.Interaction.TargetEntity = targetEntity;
                Logger.Debug("Player {0} selected {1} as target.", player.Object.Name, targetEntity.Object.Name);
            }
            else
            {
                player.Interaction.TargetEntity = null;
                Logger.Debug("Player {0} cleared selection on {1}.", player.Object.Name, targetEntity.Object.Name);
            }
        }
    }
}