using System;

namespace Rhisis.Core.Exceptions
{
    /// <summary>
    /// Represents the errors that occurred during the Rhisis applications execution.
    /// </summary>
    public class RhisisException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="RhisisException"/> with a specific error message.
        /// </summary>
        /// <param name="message">Error message</param>
        public RhisisException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new <see cref="RhisisException"/> with a specific error message and a reference to the inner exception that throwed this exception.
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="innerException">Inner exception</param>
        public RhisisException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
