using Rhisis.Core.Common;

namespace Rhisis.World.Game.Components
{
    public class PlayerComponent
    {
        public int Id { get; set; }

        public int Slot { get; set; }

        public int Gold { get; set; }

        public string CurrentShopName { get; set; }

        public AuthorityType Authority { get; set; }
    }
}
