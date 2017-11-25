using System;

namespace Rhisis.World.Systems.Events
{
    public class ChatEventArgs : EventArgs
    {
        public string Message { get; }

        public ChatEventArgs(string message)
        {
            this.Message = message;
        }
    }
}
