using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.Core.DependencyInjection;
using Rhisis.World.Game.Common;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using System;

namespace Rhisis.World.Game.Chat
{
    public class AddDamageCommand
    {
        private static ILogger Logger => DependencyContainer.Instance.Resolve<ILogger<AddDamageCommand>>();

        [ChatCommand(".adddamage", AuthorityType.Administrator)]
        [ChatCommand(".ad", AuthorityType.Administrator)]
        public static void AddDamage(IPlayerEntity player, string[] parameters)
        {
            if(parameters.Length < 1)
            {
                WorldPacketFactory.SendWorldMsg(player, "Syntax: .ad <atkFlag> <damage>");
                return;
            }

            if(player.Interaction.TargetEntity == null)
            {
                WorldPacketFactory.SendWorldMsg(player, "Select target entity first.");
                return;
            }

            Logger.LogDebug("Player {0} wants to execute AddDamage with effect to entity {1}.", player.Object.Name, player.Interaction.TargetEntity.Object.Name);

            if(!(player.Interaction.TargetEntity is ILivingEntity livingEntity))
            {
                WorldPacketFactory.SendWorldMsg(player, "Target is not a living entity.");
                return;
            }

            var atkFlag = AttackFlags.AF_GENERIC;
            var damage = 0;

            if (parameters.Length == 2)
            {
                if(!Enum.TryParse(parameters[0], out atkFlag) || !int.TryParse(parameters[1], out damage))
                {
                    WorldPacketFactory.SendWorldMsg(player, "Invalid atkFlags or damage parameter.");
                    return;
                }
            }
            else
            {
                if (!int.TryParse(parameters[0], out damage))
                {
                    WorldPacketFactory.SendWorldMsg(player, "Invalid damage parameter.");
                    return;
                }
            }

            WorldPacketFactory.SendAddDamage(livingEntity, player, atkFlag, damage);
        }
    }
}