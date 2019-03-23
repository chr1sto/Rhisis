using System;

namespace Rhisis.Core.Exceptions
{
    public class RhisisConfigurationException : RhisisException
    {
        private static readonly string ConfigurationExceptionMessage = "Cannot load configuration for file: '{0}'";

        public RhisisConfigurationException()
            : base("Cannot load configuration file.")
        {
        }

        public RhisisConfigurationException(string file) 
            : base(string.Format(ConfigurationExceptionMessage, file))
        {
        }

        public RhisisConfigurationException(string file, Exception innerException)
            : base(string.Format(ConfigurationExceptionMessage, file), innerException)
        {
        }
    }
}
