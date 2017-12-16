using System;

namespace Rhisis.World.Systems.Events
{
    public class ChatEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the chat message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Creates a <see cref="ChatEventArgs"/> instance.
        /// </summary>
        /// <param name="message">Chat message</param>
        public ChatEventArgs(string message)
        {
            this.Message = message;
        }
    }
}
