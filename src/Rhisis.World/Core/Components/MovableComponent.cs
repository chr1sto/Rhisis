using Rhisis.Core.Structures;

namespace Rhisis.World.Core.Components
{
    public class MovableComponent : IComponent
    {
        public Vector3 DestinationPosition { get; set; }

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
