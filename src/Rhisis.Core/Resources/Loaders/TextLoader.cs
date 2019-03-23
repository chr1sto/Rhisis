using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rhisis.Core.Resources.Loaders
{
    public sealed class TextLoader : IGameResourceLoader
    {
        private readonly ILogger<TextLoader> _logger;

        /// <summary>
        /// Gets the texts dicitonary.
        /// </summary>
        public IDictionary<string, string> Texts { get; }

        /// <summary>
        /// Gets the text value by key.
        /// </summary>
        /// <param name="key">Text key</param>
        /// <returns>Value if key is found; "[undefined]" otherwise.</returns>
        public string this[string key] => this.Texts.TryGetValue(key, out string value) ? value : "[undefined]";

        /// <summary>
        /// Creates a new <see cref="TextLoader"/> instance.
        /// </summary>
        /// <param name="logger">Logger</param>
        public TextLoader(ILogger<TextLoader> logger)
        {
            this._logger = logger;
            this.Texts = new Dictionary<string, string>();
        }

        /// <inheritdoc />
        public void Load()
        {
            var textFiles = from x in Directory.GetFiles(GameResources.ResourcePath, "*.*", SearchOption.AllDirectories)
                            where TextFile.Extensions.Contains(Path.GetExtension(x)) && x.EndsWith(".txt.txt")
                            select x;

            foreach (var textFilePath in textFiles)
            {
                using (var textFile = new TextFile(textFilePath))
                {
                    foreach (var text in textFile.Texts)
                    {
                        if (!Texts.ContainsKey(text.Key) && text.Value != null)
                            Texts.Add(text);
                        else
                        {
                            this._logger.LogWarning(GameResources.ObjectIgnoredMessage, "Text", text.Key,
                                (text.Value == null) ? "cannot get the value" : "already declared");
                        }
                    }
                }
            }

            this._logger.LogInformation("-> {0} texts found.", Texts.Count);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Texts.Clear();
        }
    }
}
