using System.Collections.Generic;

namespace Rhisis.World.Game.Core.Interfaces
{
    public interface IContext
    {
        IReadOnlyList<ISystem> Systems { get; }

        IReadOnlyList<IEntity> Entities { get; }

        TEntity CreateEntity<TEntity>() where TEntity : class, IEntity;

        bool DeleteEntity(IEntity entity);

        IEntity FindEntity(int id);
    }
}
