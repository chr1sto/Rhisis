using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rhisis.Core.Resources.Loaders
{
    public sealed class TextLoader : IGameResourceLoader
    {
        private readonly ILogger<TextLoader> _logger;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Creates a new <see cref="TextLoader"/> instance.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="cache">Memory cache.</param>
        public TextLoader(ILogger<TextLoader> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        /// <inheritdoc />
        public void Load()
        {
            var texts = new ConcurrentDictionary<string, string>();
            var textFiles = from x in Directory.GetFiles(GameResourcesConstants.Paths.ResourcePath, "*.*", SearchOption.AllDirectories)
                            where TextFile.Extensions.Contains(Path.GetExtension(x)) && x.EndsWith(".txt.txt")
                            select x;

            foreach (var textFilePath in textFiles)
            {
                using var textFile = new TextFile(textFilePath);
                foreach (var text in textFile.Texts)
                {
                    if (!texts.ContainsKey(text.Key) && text.Value != null)
                        texts.TryAdd(text.Key, text.Value);
                    else
                    {
                        _logger.LogWarning(GameResourcesConstants.Errors.ObjectIgnoredMessage, "Text", text.Key,
                            (text.Value == null) ? "cannot get the value" : "already declared");
                    }
                }
            }

            _cache.Set(GameResourcesConstants.Texts, texts);
            _logger.LogInformation("-> {0} texts found.", texts.Count);
        }
    }
}
