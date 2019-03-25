using System;

namespace Rhisis.Core
{
    /// <summary>
    /// Provides a mechanism to configure and run a program.
    /// </summary>
    public interface IProgramStartup : IDisposable
    {
        /// <summary>
        /// Configure the program.
        /// </summary>
        void Configure();

        /// <summary>
        /// Runs the program logic.
        /// </summary>
        void Run();
    }
}