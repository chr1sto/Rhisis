using Rhisis.Core.Structures;

namespace Rhisis.World.Core.Components
{
    public class MovableComponent : IComponent
    {
        public long MoveTime { get; set; }

        public long LastMoveTime { get; set; }
        
        public long NextMoveTime { get; set; }

        public Vector3 DestinationPosition { get; set; }

        public float Speed { get; set; }

        public MovableComponent()
        {
            this.DestinationPosition = new Vector3();
        }

        public override string ToString()
        {
            return $"Dest Position: {this.DestinationPosition.ToString()}";
        }
    }
}
