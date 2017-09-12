using System;
using System.Text;

namespace Rhisis.Core.IO
{
    /// <summary>
    /// Logger that writes messages to the console and log files.
    /// </summary>
    public static class Logger
    {
        private enum LogType
        {
            Info,
            Debug,
            Warning,
            Error,
            Loading
        }

        private static readonly object syncRoot = new object();

        /// <summary>
        /// Initialize the <see cref="Logger"/>.
        /// </summary>
        public static void Initialize()
        {
#if !NET45
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
        }

        /// <summary>
        /// Writes an information message.
        /// </summary>
        /// <param name="format">Formated text</param>
        /// <param name="args">parameters</param>
        public static void Info(string format, params object[] args)
        {
            Write(LogType.Info, string.Format(format, args));
        }

        /// <summary>
        /// Writes a debug message.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void Debug(string format, params object[] args)
        {
            // DEBUG configuration only
#if DEBUG
            Write(LogType.Debug, string.Format(format, args));
#endif
        }

        /// <summary>
        /// Writes an debug message.
        /// </summary>
        /// <param name="format">Formated text</param>
        /// <param name="args">parameters</param>
        public static void Warning(string format, params object[] args)
        {
            Write(LogType.Warning, string.Format(format, args));
        }

        /// <summary>
        /// Writes an error message.
        /// </summary>
        /// <param name="format">Formated text</param>
        /// <param name="args">parameters</param>
        public static void Error(string format, params object[] args)
        {
            Write(LogType.Error, string.Format(format, args));
        }

        /// <summary>
        /// Writes an loading message.
        /// </summary>
        /// <param name="format">Formated text</param>
        /// <param name="args">parameters</param>
        public static void Loading(string format, params object[] args)
        {
            Write(LogType.Loading, string.Format(format, args));
        }

        /// <summary>
        /// Writes the text to the console.
        /// </summary>
        /// <param name="type">Log type</param>
        /// <param name="text">Text</param>
        private static void Write(LogType type, string text)
        {
            lock (syncRoot)
            {
                switch (type)
                {
                    case LogType.Info:
                        SetTag(type.ToString(), ConsoleColor.Green);
                        Console.WriteLine(text);
                        break;
                    case LogType.Debug:
                        SetTag(type.ToString(), ConsoleColor.Blue);
                        Console.WriteLine(text);
                        break;
                    case LogType.Warning:
                        SetTag(type.ToString(), ConsoleColor.Yellow);
                        Console.WriteLine(text);
                        break;
                    case LogType.Error:
                        SetTag(type.ToString(), ConsoleColor.Red);
                        Console.WriteLine(text);
                        break;
                    case LogType.Loading:
                        SetTag(type.ToString(), ConsoleColor.DarkMagenta);
                        Console.Write(text);
                        break;
                    default:
                        Console.WriteLine(text);
                        break;
                }
            }
        }

        /// <summary>
        /// Set the tag color.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="color"></param>
        private static void SetTag(string tag, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write($"\r[{tag}] ");
            Console.ResetColor();
        }
    }
}
