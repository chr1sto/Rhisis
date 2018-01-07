using System;

namespace Rhisis.World.Game.Chat
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    internal class ChatCommandAttribute : Attribute
    {
        /// <summary>
        /// Gets the chat command.
        /// </summary>
        public string Command { get; }

        /// <summary>
        /// Creates a new <see cref="ChatCommandAttribute"/> instance.
        /// </summary>
        /// <param name="command">Chat command</param>
        public ChatCommandAttribute(string command)
        {
            this.Command = command;
        }
    }
}