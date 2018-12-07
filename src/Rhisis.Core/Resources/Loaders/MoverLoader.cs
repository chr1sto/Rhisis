using Microsoft.Extensions.Logging;
using Rhisis.Core.Structures.Game;
using System.Collections.Generic;
using System.IO;

namespace Rhisis.Core.Resources.Loaders
{
    public sealed class MoverLoader : IGameResourceLoader
    {
        private readonly ILogger<MoverLoader> _logger;
        private readonly DefineLoader _definesLoader;
        private readonly TextLoader _textsLoader;
        private readonly IDictionary<int, MoverData> _moversData;

        /// <summary>
        /// Gets a <see cref="MoverData"/> by his mover id.
        /// </summary>
        /// <param name="moverId"></param>
        /// <returns></returns>
        public MoverData this[int moverId] => this.GetMover(moverId);

        /// <summary>
        /// Creates a new <see cref="MoverLoader"/> instance.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="defines">Defines data</param>
        /// <param name="texts">Texts data</param>
        public MoverLoader(ILogger<MoverLoader> logger, DefineLoader defines, TextLoader texts)
        {
            this._logger = logger;
            this._definesLoader = defines;
            this._textsLoader = texts;
            this._moversData = new Dictionary<int, MoverData>();
        }

        /// <inheritdoc />
        public void Load()
        {
            if (!File.Exists(GameResources.MoversPropPath))
            {
                this._logger.LogWarning("Unable to load movers. Reason: cannot find '{0}' file.", GameResources.MoversPropPath);
                return;
            }

            using (var moversPropFile = new ResourceTableFile(GameResources.MoversPropPath, 1, this._definesLoader.Defines, this._textsLoader.Texts))
            {
                var movers = moversPropFile.GetRecords<MoverData>();

                foreach (var mover in movers)
                {
                    if (this._moversData.ContainsKey(mover.Id))
                    {
                        this._moversData[mover.Id] = mover;
                        this._logger.LogWarning(GameResources.ObjectOverridedMessage, "Mover", mover.Id, "already declared");
                    }
                    else
                        this._moversData.Add(mover.Id, mover);
                }
            }

            this._logger.LogInformation("-> {0} movers loaded.", this._moversData.Count);
        }

        /// <summary>
        /// Gets a <see cref="MoverData"/> by his mover id.
        /// </summary>
        /// <param name="moverId"></param>
        /// <returns></returns>
        public MoverData GetMover(int moverId) => this._moversData.TryGetValue(moverId, out MoverData value) ? value : null;

        /// <inheritdoc />
        public void Dispose()
        {
            this._moversData.Clear();
        }
    }
}
