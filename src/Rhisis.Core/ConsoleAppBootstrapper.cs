using System;
using System.Globalization;

namespace Rhisis.Core
{
    public class ConsoleAppBootstrapper : IDisposable
    {
        private IProgramStartup _startup;

        private ConsoleAppBootstrapper()
        {
        }

        /// <summary>
        /// Creates a new <see cref="ConsoleAppBootstrapper"/> instance.
        /// </summary>
        /// <returns></returns>
        public static ConsoleAppBootstrapper CreateApp() => new ConsoleAppBootstrapper();

        /// <summary>
        /// Sets the console title.
        /// </summary>
        /// <param name="title">Console title</param>
        /// <returns></returns>
        public ConsoleAppBootstrapper SetConsoleTitle(string title)
        {
            Console.Title = title;

            return this;
        }

        /// <summary>
        /// Sets the current program culture.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public ConsoleAppBootstrapper SetCulture(string culture)
        {
            CultureInfo.CurrentCulture = new CultureInfo(culture);
            CultureInfo.CurrentUICulture = new CultureInfo(culture);
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(culture);
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(culture);

            return this;
        }

        /// <summary>
        /// Defines the program startup class.
        /// </summary>
        /// <typeparam name="TStartup"></typeparam>
        /// <returns></returns>
        public ConsoleAppBootstrapper UseStartup<TStartup>() where TStartup : class, IProgramStartup
        {
            this._startup = Activator.CreateInstance<TStartup>() as IProgramStartup;
            this._startup.Configure();

            AppDomain.CurrentDomain.ProcessExit += (sender, args) => this._startup.Dispose();

            return this;
        }

        /// <summary>
        /// Start the program.
        /// </summary>
        public void Run()
        {
            if (this._startup == null)
                throw new InvalidProgramException("No startup class specified.");

            this._startup.Run();
        }

        /// <summary>
        /// Releases the program resources.
        /// </summary>
        public void Dispose() => this._startup?.Dispose();
    }
}