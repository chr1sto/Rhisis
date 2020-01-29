using Microsoft.Extensions.Logging;
using System;

namespace Rhisis.Core.Extensions
{
    public static class LoggerExtensions
    {
        /// <summary>
        /// Logs a loading message to the console.
        /// </summary>
        /// <param name="logger">Current logger.</param>
        /// <param name="message">Message format.</param>
        /// <param name="args">Message arguments.</param>
        public static void LogLoading(this ILogger logger, string message, params object[] args)
        {
            WriteColoredMessage(ConsoleColor.White, $"[{ DateTime.Now :yyyy-MM-dd HH:mm:ss.ffff}] ");
            WriteColoredMessage(ConsoleColor.DarkMagenta, "[LOADING] ");
            WriteColoredMessage(ConsoleColor.White, message, args);
        }

        /// <summary>
        /// Writes a colored message to the console.
        /// </summary>
        /// <param name="color">Message color.</param>
        /// <param name="message">Message format.</param>
        /// <param name="args">Message arguments.</param>
        private static void WriteColoredMessage(ConsoleColor color, string message, params object[] args)
        {
            Console.ForegroundColor = color;
            Console.Write(message, args);
            Console.ResetColor();
        }

        /// <summary>
        /// Clears the current line of the console.
        /// </summary>
        /// <param name="logger">Current logger.</param>
        /// <param name="initialStartLine">The line the message started at.</param>
        /// <param name="linesToErase">How many lines to clear from the start position.</param>
        public static void ClearCurrentConsoleLine(this ILogger logger, int initialStartLine, int linesToErase = 2)
        {
            Console.SetCursorPosition(0, initialStartLine);
            for (int i = 0; i < Console.WindowWidth * linesToErase; i++)
                Console.Write(" ");
            Console.SetCursorPosition(0, initialStartLine);
        }
    }
}
