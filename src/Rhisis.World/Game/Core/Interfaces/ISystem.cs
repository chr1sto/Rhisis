namespace Rhisis.World.Game.Core.Interfaces
{
    public interface ISystem
    {
        void Execute(IEntity entity);

        bool Match(IEntity entity);
    }
}
