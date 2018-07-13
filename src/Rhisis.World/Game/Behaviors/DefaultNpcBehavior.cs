using Rhisis.Core.Helpers;
using Rhisis.Core.IO;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Game.Behaviors
{
    /// <summary>
    /// Default behavior of a NPC.
    /// </summary>
    [Behavior(BehaviorType.Npc, IsDefault: true)]
    public class DefaultNpcBehavior : IBehavior<INpcEntity>
    {
        /// <inheritdoc />
        public void Update(INpcEntity entity)
        {
            this.UpdateOralText(entity);
        }

        private void UpdateOralText(INpcEntity npc)
        {
            if (npc.Timers.LastSpeakTime <= Time.TimeInSeconds())
            {
                if (npc.Data.HasDialog && !string.IsNullOrEmpty(npc.Data.Dialog.OralText))
                {
                    IEnumerable<IPlayerEntity> playersArount = from x in npc.Object.Entities
                                                               where x.Object.Position.IsInCircle(npc.Object.Position, 60) &&
                                                               x is IPlayerEntity
                                                               select x as IPlayerEntity;

                    foreach (IPlayerEntity player in playersArount)
                    {
                        string text = npc.Data.Dialog.OralText.Replace("%PLAYERNAME%", player.Object.Name);

                        WorldPacketFactory.SendChatTo(npc, player, text);
                    }

                    npc.Timers.LastSpeakTime = Time.TimeInSeconds() + RandomHelper.Random(10, 15);
                }
            }
        }
    }
}
