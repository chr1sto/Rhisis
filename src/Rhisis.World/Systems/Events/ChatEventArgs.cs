using System;
using Rhisis.World.Game.Core;

namespace Rhisis.World.Systems.Events
{
    public sealed class ChatEventArgs : SystemEventArgs
    {
        /// <summary>
        /// Gets the chat message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Creates a <see cref="ChatEventArgs"/> instance.
        /// </summary>
        /// <param name="args">System arguments</param>
        public ChatEventArgs(params object[] args)
            : base(args)
        {
            this.Message = this.GetArgument<string>(0);
        }

        /// <summary>
        /// Check system arguments.
        /// </summary>
        /// <returns></returns>
        public override bool CheckArguments() => !string.IsNullOrEmpty(this.Message);
    }
}
