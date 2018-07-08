using Rhisis.World.Game.Core;

namespace Rhisis.World.Game.Behaviors
{
    public interface IBehavior<in T> where T : IEntity
    {
        void Update(T entity);
    }
}
