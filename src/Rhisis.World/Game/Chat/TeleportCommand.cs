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

            if (parameters.Length >= 2 && parameters.Length <= 4)
            {
                float.TryParse(parameters[0], out float firstValue);
                float.TryParse(parameters[1], out float secondValue);
                player.Object.Position.X = firstValue;

                if (parameters.Length == 2)
                {
                    player.Object.Position.Z = secondValue;
                    WorldPacketFactory.SendPlayerTeleport(player);
                    return;
                }

                int.TryParse(parameters[0], out int mapIdValue);

                /*IReadOnlyDictionary<int, Map> worldId = new Dictionary<int, Map>();

                if (!worldId.TryGetValue(mapIdValue, out Map MapId))
                {
                    Logger.Warning("Cannot find map Id in define files: {0}. Please check you defineWorld.h file.",
                        worldId);
                    return;
                }*/

                float.TryParse(parameters[2], out float thirdValue);

                if (parameters.Length == 3)
                {
                    player.Object.MapId = mapIdValue;
                    player.Object.Position.X = secondValue;
                    player.Object.Position.Z = thirdValue;
                    WorldPacketFactory.SendPlayerTeleport(player);
                    return;
                }

                float.TryParse(parameters[3], out float forthValue);

                if (parameters.Length == 4)
                {
                    player.Object.MapId = mapIdValue;
                    player.Object.Position.X = secondValue;
                    player.Object.Position.Y = thirdValue;
                    player.Object.Position.Z = forthValue;
                    WorldPacketFactory.SendPlayerTeleport(player);
                    return;
                }
            }
            else
            {
                Logger.Error("Chat: /teleport command must have 2, 3 or 4 parameters.");
            }
        }
    }
}