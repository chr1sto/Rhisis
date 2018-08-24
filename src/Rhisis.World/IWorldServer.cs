using Rhisis.World.Game.Entities;

namespace Rhisis.World
{
    public interface IWorldServer
    {
        IPlayerEntity GetPlayerEntity(int id);
    }
}
