using NLog;
using Rhisis.Core.Common;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;

namespace Rhisis.World.Game.Chat
{
    public static class AddDamageCommand
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        [ChatCommand("/AddDamage", AuthorityType.Administrator)]
        [ChatCommand("/ad", AuthorityType.Administrator)]
        public static void AddDamage(IPlayerEntity player, string[] parameters)
        {
            //WorldPacketFactory.SendAddDamage(player, )
        }
    }
}