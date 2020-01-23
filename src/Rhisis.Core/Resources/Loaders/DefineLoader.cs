using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;

namespace Rhisis.Core.Resources.Loaders
{
    public sealed class DefineLoader : IGameResourceLoader
    {
        private readonly ILogger<DefineLoader> _logger;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Creates a new <see cref="DefineLoader"/> instance.
        /// </summary>
        /// <param name="logger">Logger</param>
        public DefineLoader(ILogger<DefineLoader> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        /// <inheritdoc />
        public void Load()
        {
            var defines = new ConcurrentDictionary<string, int>();
            var headerFiles = from x in Directory.GetFiles(GameResourcesConstants.Paths.ResourcePath, "*.*", SearchOption.AllDirectories)
                              where DefineFile.Extensions.Contains(Path.GetExtension(x))
                              select x;

            foreach (var headerFile in headerFiles)
            {
                using (var defineFile = new DefineFile(headerFile))
                {
                    foreach (var define in defineFile.Defines)
                    {
                        var isIntValue = int.TryParse(define.Value.ToString(), out int intValue);

                        if (isIntValue && !defines.ContainsKey(define.Key))
                            defines.TryAdd(define.Key, intValue);
                        else
                        {
                            _logger.LogWarning(GameResourcesConstants.Errors.ObjectIgnoredMessage, "Define", define.Key,
                                isIntValue ? "already declared" : $"'{define.Value}' is not a integer value");
                        }
                    }
                }
            }

            _cache.Set(GameResourcesConstants.Defines, defines);
            _logger.LogInformation("-> {0} defines found.", defines.Count);
        }
    }
}
