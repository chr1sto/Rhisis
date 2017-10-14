using Rhisis.Core.Helpers;
using Rhisis.Core.Structures;
using Rhisis.World.Core.Entities;
using System.Collections.Generic;

namespace Rhisis.World.Core.Components
{
    public class ObjectComponent : IComponent
    {
        private readonly int _objectId;

        /// <summary>
        /// Gets the unique object id.
        /// </summary>
        public int ObjectId => this._objectId;

        /// <summary>
        /// Gets or sets the map id.
        /// </summary>
        public int MapId { get; set; }

        /// <summary>
        /// Gets or sets the object position.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the object orientation angle.
        /// </summary>
        public float Angle { get; set; }

        /// <summary>
        /// Gets or sets the object size.
        /// </summary>
        public short Size { get; set; }

        /// <summary>
        /// Gets the list of the visible entities around.
        /// </summary>
        public IList<IEntity> Entities { get; private set; }

        /// <summary>
        /// Creates and initializes a new <see cref="ObjectComponent"/>.
        /// </summary>
        public ObjectComponent()
        {
            this._objectId = RandomHelper.GenerateUniqueId();
            this.Position = new Vector3();
            this.Entities = new List<IEntity>();
        }
    }
}
