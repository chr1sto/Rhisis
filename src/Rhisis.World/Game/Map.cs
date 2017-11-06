using Rhisis.World.Core;
using Rhisis.World.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rhisis.World.Game
{
    /// <summary>
    /// Defines a map with it's own entities and context.
    /// </summary>
    public sealed class Map : IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;

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
            this._cancellationTokenSource = new CancellationTokenSource();
            this._cancellationToken = this._cancellationTokenSource.Token;
        }

        /// <summary>
        /// Start the map update task.
        /// </summary>
        public void Start()
        {
            Task.Factory.StartNew(() => this.Update());
        }

        private async Task Update()
        {
            while (true)
            {
                if (this._cancellationToken.IsCancellationRequested)
                    break;

                foreach (var system in this.Context.Systems)
                    system.Execute();
                
                await Task.Delay(50);
            }
        }
        
        public void Dispose()
        {
            if (this._isDisposed)
                throw new ObjectDisposedException(nameof(Map));

            this._cancellationTokenSource.Cancel();
            this._isDisposed = true;
        }

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
