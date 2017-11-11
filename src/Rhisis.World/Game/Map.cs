using Rhisis.World.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rhisis.World.Game
{
    /// <summary>
    /// Defines a map with it's own entities and context.
    /// </summary>
    public sealed class Map : IDisposable
    {
        private bool _isDisposed;

        /// <summary>
        /// Gets the map id.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the map context.
        /// </summary>
        public IContext Context { get; }

        /// <summary>
        /// Creates and initializes a new <see cref="Map"/> instance.
        /// </summary>
        public Map()
        {
            this.Context = new Context();
        }

        /// <summary>
        /// Start the map update task.
        /// </summary>
        public void Start()
        {
            this.Context.StartSystemUpdate(50);
        }
        
        /// <summary>
        /// Dispose the map resources.
        /// </summary>
        public void Dispose()
        {
            if (!this._isDisposed)
            {
                this.Context.Dispose();
                this._isDisposed = true;
            }
        }

        /// <summary>
        /// Loads a new map.
        /// </summary>
        /// <param name="mapPath">Map path</param>
        /// <returns>New map</returns>
        public static Map Load(string mapPath)
        {
            // TODO: load map informations
            // TODO: load regions
            // TODO: load objects
            // TODO: load heights
            // TODO: load revival zones

            return new Map();
        }
    }
}
