using Rhisis.Core.Structures.Game;
using Rhisis.World.Game.Maps.Regions;

namespace Rhisis.World.Game.Entities
{
    public interface IMonsterEntity : IWorldEntity, IMovableEntity, ILivingEntity
    {
        /// <summary>
        /// Gets or sets the parent region of the monster.
        /// </summary>
        IMapRespawnRegion Region { get; set; }
    }
}
