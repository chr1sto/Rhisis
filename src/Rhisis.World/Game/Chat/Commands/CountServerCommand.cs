using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.Core.Data;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;

namespace Rhisis.World.Game.Chat
{
    [ChatCommand("/count", AuthorityType.GameMaster)]
    [ChatCommand("/cnt", AuthorityType.GameMaster)]
    public class CountCommand : IChatCommand
    {
        private readonly IWorldServer _worldServer;
        private readonly ITextPacketFactory _textPacketFactory;

        /// <summary>
        /// Creates a new <see cref="CountCommand"/> instance.
        /// </summary>
        /// <param name="worldServer">World server.</param>
        /// <param name="textPacketFactory">Text packet factory.</param>
        public CountCommand(IWorldServer worldServer, ITextPacketFactory textPacketFactory)
        {
            _worldServer = worldServer;
            _textPacketFactory = textPacketFactory;
        }

        /// <inheritdoc />
        public void Execute(IPlayerEntity player, object[] parameters)
        {
            uint onlineCount = _worldServer.GetOnlineConnectedPlayerNumber();
            string countMessage = string.Format("Players Online : {0}", onlineCount);
            _textPacketFactory.SendWorldMessage(player, countMessage);
        }
    }
} 