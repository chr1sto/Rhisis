using Sylver.Network.Data;
using System;

namespace Rhisis.World.Game.Structures
{
    public class QuestInfo
    {
        public int QuestId { get; }

        public int? DatabaseQuestId { get; }

        public int CharacterId { get; }

        public bool IsFinished { get; set; }

        public bool IsChecked { get; set; }

        public DateTime StartTime { get; set; }

        public QuestInfo(int questId, int characterId)
            : this(questId, characterId, default)
        {
        }

        public QuestInfo(int questId, int characterId, int? databaseQuestId)
        {
            this.QuestId = questId;
            this.CharacterId = characterId;
            this.DatabaseQuestId = databaseQuestId;
        }

        public void Serialize(INetPacketStream packet)
        {
            packet.Write<short>(0); // state
            packet.Write<short>(0); // time limit
            packet.Write((short)this.QuestId);

            packet.Write<short>(0); // monster 1 killed
            packet.Write<short>(0); // monster 2 killed
            packet.Write<byte>(0); // patrol done
            packet.Write<byte>(0); // dialog done
        }
    }
}