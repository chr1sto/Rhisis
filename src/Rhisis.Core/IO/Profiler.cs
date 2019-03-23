using System;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Rhisis.Core.IO
{
    public static class Profiler
    {
        private static readonly ConcurrentDictionary<string, Stopwatch> _watches = new ConcurrentDictionary<string, Stopwatch>();

        public static void Start(string actionName)
        {
            if (_watches.TryGetValue(actionName, out Stopwatch watch) && !watch.IsRunning)
            {
                watch.Start();
            }
            else
            {
                var newProfilerWatch = new Stopwatch();

                if (!_watches.TryAdd(actionName, newProfilerWatch))
                {
#if DEBUG
                    throw new InvalidOperationException();
#else
                    // Nothing to do.
#endif
                }

                newProfilerWatch.Start();
            }
        }

        public static Stopwatch Stop(string actionName)
        {
            if (_watches.TryGetValue(actionName, out Stopwatch watch) && watch.IsRunning)
            {
                watch.Stop();
                return watch;
            }

            return null;
        }
    }
}
