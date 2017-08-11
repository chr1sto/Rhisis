using System;

namespace Rhisis.Database.Exceptions
{
    /// <summary>
    /// Represents the errors that occurred during the Rhisis database context.
    /// </summary>
    public class RhisisDatabaseException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="RhisisDatabaseException"/> with a specific error message.
        /// </summary>
        /// <param name="message">Error message</param>
        public RhisisDatabaseException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new <see cref="RhisisDatabaseException"/> with a specific error message and a reference to the inner exception that throwed this exception.
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="innerException">Inner exception</param>
        public RhisisDatabaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
