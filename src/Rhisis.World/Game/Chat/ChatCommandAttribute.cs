using Rhisis.Core.Common;
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
        /// Gets the chat commands minimum authorization.
        /// </summary>
        public AuthorityType MinAuthorization { get; }

        /// <summary>
        /// Creates a new <see cref="ChatCommandAttribute"/> instance.
        /// </summary>
        /// <param name="command">Chat command</param>
        /// <param name="minAuthorization">Minimum Authorization</param>
        public ChatCommandAttribute(string command, AuthorityType minAuthorization)
        {
            this.Command = command;
            this.MinAuthorization = minAuthorization;
        }
    }
}