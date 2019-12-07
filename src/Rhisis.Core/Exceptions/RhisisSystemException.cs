using System;

namespace Rhisis.Core.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a Rhisis exception in an ECS System
    /// </summary>
    public class RhisisSystemException : RhisisException
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates a new <see cref="T:Rhisis.Core.Exceptions.SystemException" /> with a specific error message.
        /// </summary>
        /// <param name="message">Error message</param>
        public RhisisSystemException(string message) :
            base(message)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates a new <see cref="T:Rhisis.Core.Exceptions.SystemException" /> with a specific error message and a reference to the inner exception that throwed this exception.
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="innerException">Inner exception</param>
        public RhisisSystemException(string message, Exception innerException) :
            base(message, innerException)
        {
        }
    }
}
