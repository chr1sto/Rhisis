using Rhisis.World.Game.Core;
using System.Collections;
using System.Linq;

namespace Rhisis.World.Game.Components
{
    public class FollowComponent
    {
        /// <summary>
        /// Gets or sets the following target.
        /// </summary>
        public IEntity Target { get; set; }

        /// <summary>
        /// Gets or sets if the object is following another.
        /// </summary>
        public bool IsFollowing => this.Target != null;
    }
}
