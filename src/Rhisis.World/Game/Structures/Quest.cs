using Sylver.Network.Data;

namespace Rhisis.World.Game.Structures
{
    public class Quest
    {
        public int QuestId { get; }

        public int DatabaseQuestId { get; }

        public int CharacterId { get; }

        public bool IsFinished { get; set; }

        public bool IsChecked { get; set; }

        public Quest(int questId, int databaseQuestId, int characterId)
        {
            this.QuestId = questId;
            this.DatabaseQuestId = databaseQuestId;
            this.CharacterId = characterId;
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