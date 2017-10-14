using Ether.Network;

namespace Rhisis.World.Core.Components
{
    public class PlayerComponent : IComponent
    {
        public int Id { get; set; }

        public int Slot { get; set; }

        public NetConnection Connection { get; set; }
    }
}
