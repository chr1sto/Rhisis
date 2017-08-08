using System;

namespace Rhisis.Core.Exceptions
{
    public class RhisisPacketException : RhisisException
    {
        public RhisisPacketException(string message) 
            : base(message)
        {
        }

        public RhisisPacketException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
