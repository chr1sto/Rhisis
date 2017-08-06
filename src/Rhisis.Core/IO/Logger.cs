using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.Core.IO
{
    public static class Logger
    {
        private enum LoggerType
        {
            Info,
            Debug,
            Warning,
            Error
        }

        public static void Initialize()
        {
            throw new NotImplementedException();
        }

        public static void Info(string format, params object[] args)
        {
            Write(LoggerType.Info, string.Format(format, args));
        }

        public static void Debug(string format, params object[] args)
        {
            Write(LoggerType.Debug, string.Format(format, args));
        }

        public static void Warning(string format, params object[] args)
        {
            Write(LoggerType.Warning, string.Format(format, args));
        }

        public static void Error(string format, params object[] args)
        {
            Write(LoggerType.Error, string.Format(format, args));
        }

        private static void Write(LoggerType type, string text)
        {
            throw new NotImplementedException();
        }
    }
}
