using Rhisis.World.Core;
using Rhisis.World.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rhisis.World.Game
{
    /// <summary>
    /// Defines a map with it's own entities and context.
    /// </summary>
    public sealed class Map
    {
        private readonly Task _updateTask;

        /// <summary>
        /// Gets the map id.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the map context.
        /// </summary>
        public IContext Context { get; }

        public Map()
        {
            this.Context = new Context();
        }

        public void Start()
        {
            // TODO: start a new task that will update all systems of the current map context
        }

        public static Map Load(string mapPath)
        {
            return new Map();
        }
    }
}
