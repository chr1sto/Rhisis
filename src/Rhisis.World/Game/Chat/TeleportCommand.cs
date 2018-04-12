using Rhisis.Core.IO;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;

namespace Rhisis.World.Game.Chat
{
    public static class TeleportationCommand
    {
        [ChatCommand("/tp")]
        [ChatCommand("/teleport")]
        public static void TeleportCommand(IPlayerEntity player, string[] parameters)
        {
            Logger.Debug("{0} want to teleport", player.Object.Name);

            switch (parameters.Length)
            {
                case 2: // when you write 2 parameters in the command
                    TeleportationParameters.TeleportCommandTwoParam(player, parameters);
                    return;
                case 3: // when you write 3 parameters in the command
                    TeleportationParameters.TeleportCommandThreeParam(player, parameters);
                    return;
                case 4: // when you write 4 parameters in the command
                    TeleportationParameters.TeleportCommandFourParam(player, parameters);
                    return;
                default: // when you write more than 4 or less than 2 parameters in the command
                    Logger.Error("Chat: /teleport command must have 2, 3 or 4 parameters.");
                    return;
            }
        }
    }
    public static class TeleportationParameters
    {
        public static void TeleportCommandTwoParam(IPlayerEntity player, string[] parameters)
        {
            float.TryParse(parameters[0], out float posXValue);
            float.TryParse(parameters[1], out float posZValue);
            player.Object.Position.X = posXValue;
            player.Object.Position.Z = posZValue;
            WorldPacketFactory.SendPlayerTeleport(player);
        }
        public static void TeleportCommandThreeParam(IPlayerEntity player, string[] parameters)
        {
            int.TryParse(parameters[0], out int mapIdValue);
            float.TryParse(parameters[1], out float posXValue);
            float.TryParse(parameters[2], out float posZValue);
            if (!WorldServer.Maps.TryGetValue(mapIdValue, out Map mapIdResult))
            {
                Logger.Error("Cannot find map Id in define files: {0}. Please check you defineWorld.h file.",
                    mapIdValue);
            }
            else
            {
                player.Object.MapId = mapIdValue;
                player.Object.Position.X = posXValue;
                player.Object.Position.Z = posZValue;
                WorldPacketFactory.SendPlayerTeleport(player);
            }
        }
        public static void TeleportCommandFourParam(IPlayerEntity player, string[] parameters)
        {
            int.TryParse(parameters[0], out int mapIdValue);
            float.TryParse(parameters[1], out float posXValue);
            float.TryParse(parameters[2], out float posYValue);
            float.TryParse(parameters[3], out float posZValue);
            player.Object.MapId = mapIdValue;
            player.Object.Position.X = posXValue;
            player.Object.Position.Y = posYValue;
            player.Object.Position.Z = posZValue;
            WorldPacketFactory.SendPlayerTeleport(player);
        }
    }
}