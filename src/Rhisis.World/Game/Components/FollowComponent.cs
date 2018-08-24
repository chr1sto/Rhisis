using Rhisis.World.Game.Core;

namespace Rhisis.World.Game.Components
{
    public class FollowComponent
    {
        public IEntity Target { get; set; }

        public bool IsFollowing => this.Target != null;
    }
}
