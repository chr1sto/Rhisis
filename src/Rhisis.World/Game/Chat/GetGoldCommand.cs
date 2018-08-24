using NLog;
using Rhisis.Core.Common;
using Rhisis.Core.Data;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;

namespace Rhisis.World.Game.Chat
{
    public static class GetGoldChatCommand
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        [ChatCommand("/getgold", AuthorityType.Administrator)]
        public static void GetGoldCommand(IPlayerEntity player, string[] parameters)
        {
            Logger.Debug("{0} want to get penyas", player.Object.Name);

            if (parameters.Length == 1)
            {
                if (int.TryParse(parameters[0], out int GoldByCommand))
                {
                    player.PlayerData.Gold += GoldByCommand;
                    WorldPacketFactory.SendUpdateAttributes(player, DefineAttributes.GOLD, player.PlayerData.Gold);
                }
            }
            else
            {
                Logger.Error("Chat: /getgold command must have only one parameter.");
            }
        }
    }
}