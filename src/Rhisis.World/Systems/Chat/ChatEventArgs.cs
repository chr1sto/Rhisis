using Rhisis.World.Game.Core.Systems;

namespace Rhisis.World.Systems.Chat
{
    public sealed class ChatEventArgs : SystemEventArgs
    {
        /// <summary>
        /// Gets the chat message.
        /// </summary>
        public string Message { get; }

        /// <inheritdoc />
        /// <summary>
        /// Creates a <see cref="T:Rhisis.World.Systems.Chat.ChatEventArgs" /> instance.
        /// </summary>
        /// <param name="message"></param>
        public ChatEventArgs(string message)
        {
            this.Message = message;
        }

        /// <inheritdoc />
        public override bool GetCheckArguments()
        {
            return !string.IsNullOrEmpty(this.Message);
        }
    }
}
