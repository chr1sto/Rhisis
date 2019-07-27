using Rhisis.Core.Structures.Game;
using System;
using System.Collections.Generic;

namespace Rhisis.Core.Resources
{
    /// <summary>
    /// Provides a mechanism to load data from resource files.
    /// </summary>
    public interface IGameResources
    {
        /// <summary>
        /// Gets the movers data.
        /// </summary>
        IReadOnlyDictionary<int, MoverData> Movers { get; }
        
        /// <summary>
        /// Gets the items data.
        /// </summary>
        IReadOnlyDictionary<int, ItemData> Items { get; }

        /// <summary>
        /// Gets the jobs data.
        /// </summary>
        IReadOnlyDictionary<int, JobData> Jobs { get; }

        /// <summary>
        /// Load all loaders passed as parameter.
        /// </summary>
        /// <param name="loaders">Loader types.</param>
        void Load(params Type[] loaders);
    }
}
