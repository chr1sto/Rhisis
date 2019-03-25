using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rhisis.Core.Resources.Loaders
{
    public sealed class DefineLoader : IGameResourceLoader
    {
        private readonly ILogger<DefineLoader> _logger;

        /// <summary>
        /// Gets the defines dictionary.
        /// </summary>
        public IDictionary<string, int> Defines { get; }

        /// <summary>
        /// Creates a new <see cref="DefineLoader"/> instance.
        /// </summary>
        /// <param name="logger">Logger</param>
        public DefineLoader(ILogger<DefineLoader> logger)
        {
            this._logger = logger;
            this.Defines = new Dictionary<string, int>();
        }

        /// <inheritdoc />
        public void Load()
        {
            var headerFiles = from x in Directory.GetFiles(GameResources.ResourcePath, "*.*", SearchOption.AllDirectories)
                              where DefineFile.Extensions.Contains(Path.GetExtension(x))
                              select x;

            foreach (var headerFile in headerFiles)
            {
                using (var defineFile = new DefineFile(headerFile))
                {
                    foreach (var define in defineFile.Defines)
                    {
                        var isIntValue = int.TryParse(define.Value.ToString(), out int intValue);

                        if (isIntValue && !this.Defines.ContainsKey(define.Key))
                            this.Defines.Add(define.Key, intValue);
                        else
                        {
                            this._logger.LogWarning(GameResources.ObjectIgnoredMessage, "Define", define.Key,
                                isIntValue ? "already declared" : $"'{define.Value}' is not a integer value");
                        }
                    }
                }
            }

            this._logger.LogInformation("-> {0} defines found.", this.Defines.Count);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Defines.Clear();
        }
    }
}
