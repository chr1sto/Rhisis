using Rhisis.Core.IO;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Maps;
using Rhisis.World.Packets;
using System;

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
                    break;
                case 3: // when you write 3 parameters in the command
                    TeleportationParameters.TeleportCommandThreeParam(player, parameters);
                    break;
                case 4: // when you write 4 parameters in the command
                    TeleportationParameters.TeleportCommandFourParam(player, parameters);
                    break;
                default: // when you write more than 4 or less than 2 parameters in the command
                    Logger.Error("Chat: /teleport command must have 2, 3 or 4 parameters.");
                    return;
            }
            WorldPacketFactory.SendPlayerTeleport(player);
        }
    }
    internal static class TeleportationParameters
    {
        public static void TeleportCommandTwoParam(IPlayerEntity player, string[] parameters)
        {
            if (!float.TryParse(parameters[0], out float posXValue) || !float.TryParse(parameters[1], out float posZValue))
            {
                throw new ArgumentException("You must write numbers for teleport command's parameters");
            }

            player.Object.Position.X = posXValue;
            player.Object.Position.Z = posZValue;
        }
        public static void TeleportCommandThreeParam(IPlayerEntity player, string[] parameters)
        {
            if (!ushort.TryParse(parameters[0], out ushort mapIdValue) || !float.TryParse(parameters[1], out float posXValue)
                || !float.TryParse(parameters[2], out float posZValue))
            {
                throw new ArgumentException("You must write numbers for teleport command's parameters");
            }

            if (!WorldServer.Maps.TryGetValue(mapIdValue, out IMapInstance mapIdResult))
            {
                Logger.Error("Cannot find map Id in define files: {0}. Please check you defineWorld.h file.",
                    mapIdValue);
                return;
            }

            player.Object.MapId = mapIdValue;
            player.Object.Position.X = posXValue;
            player.Object.Position.Z = posZValue;
        }
        public static void TeleportCommandFourParam(IPlayerEntity player, string[] parameters)
        {
            if (!ushort.TryParse(parameters[0], out ushort mapIdValue) || !float.TryParse(parameters[1], out float posXValue)
            || !float.TryParse(parameters[2], out float posYValue) || !float.TryParse(parameters[3], out float posZValue))
            {
                throw new ArgumentException("You must write numbers for teleport command's parameters");
            }

            if (!WorldServer.Maps.TryGetValue(mapIdValue, out IMapInstance mapIdResult))
            {
                Logger.Error("Cannot find map Id in define files: {0}. Please check you defineWorld.h file.",
                    mapIdValue);
                return;
            }

            player.Object.MapId = mapIdValue;
            player.Object.Position.X = posXValue;
            player.Object.Position.Y = posYValue;
            player.Object.Position.Z = posZValue;
        }
    }
}