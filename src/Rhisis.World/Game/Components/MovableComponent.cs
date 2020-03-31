using Rhisis.Core.Structures;

namespace Rhisis.World.Game.Components
{
    public class MovableComponent
    {
        public long MoveTime { get; set; }

        public long LastMoveTime { get; set; }
        
        public long NextMoveTime { get; set; }
        
        public Vector3 DestinationPosition { get; set; }

        public bool HasArrived => DestinationPosition.IsZero();

        public float Speed { get; set; }

        public float SpeedFactor { get; set; }

        public Vector3 BeginPosition { get; set; }

        public bool ReturningToOriginalPosition { get; set; }

        public bool IsMovingWithKeyboard { get; set; }

        public MovableComponent()
        {
            DestinationPosition = new Vector3();
            BeginPosition = new Vector3();
            SpeedFactor = 1f;
        }
    }
}
