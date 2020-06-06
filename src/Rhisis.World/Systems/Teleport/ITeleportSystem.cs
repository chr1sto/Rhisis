using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Maps;

namespace Rhisis.World.Systems.Teleport
{
    public interface ITeleportSystem
    {
        void Teleport(IPlayerEntity player, int mapId, float x, float? y, float z);

        void ChangePosition(IPlayerEntity player, IMapInstance map, float x, float? y, float z);
    }
}
