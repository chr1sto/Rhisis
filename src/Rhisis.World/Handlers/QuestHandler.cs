using Microsoft.Extensions.Logging;
using Rhisis.Network.Packets;
using Rhisis.Network.Packets.World;
using Rhisis.World.Client;
using Rhisis.World.Systems.Quest;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.World.Handlers
{
    /// <summary>
    /// Handles all quest packets.
    /// </summary>
    [Handler]
    public class QuestHandler
    {
        private readonly ILogger<QuestHandler> _logger;
        private readonly IQuestSystem _questSystem;

        public QuestHandler(ILogger<QuestHandler> logger, IQuestSystem questSystem)
        {
            _logger = logger;
            _questSystem = questSystem;
        }

        [HandlerAction(PacketType.QUEST_CHECK)]
        public void OnQuestCheck(IWorldServerClient serverClient, QuestCheckPacket questCheckPacket)
        {
            if (questCheckPacket.QuestId <= 0)
            {
                _logger.LogError($"OnQuestCheck(): Invalid quest id.");
                return;
            }

            _questSystem.CheckQuest(serverClient.Player, questCheckPacket.QuestId, questCheckPacket.Checked);
        }
    }
}
