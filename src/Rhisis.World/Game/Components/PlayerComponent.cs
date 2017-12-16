using Ether.Network;

namespace Rhisis.World.Game.Components
{
    public class PlayerComponent
    {
        public int Id { get; set; }

        public int Slot { get; set; }

        public NetConnection Connection { get; set; }
    }
}
