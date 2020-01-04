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
            WriteColoredMessage(ConsoleColor.White, $"[{ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff") }] ");
            WriteColoredMessage(ConsoleColor.DarkMagenta, "[LOADING] ");
            WriteColoredMessage(ConsoleColor.White, message, args);
            Console.Write("\r");
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
            Console.Write(string.Format(message, args));
            Console.ResetColor();
        }

        /// <summary>
        /// Clears the current line of the console.
        /// </summary>
        public static void ClearCurrentConsoleLine(this ILogger logger)
        {
            int currentLineCursor = Console.CursorTop;

            Console.SetCursorPosition(0, Console.CursorTop);
            for (int i = 0; i < Console.WindowWidth; i++)
                Console.Write(" ");
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}
