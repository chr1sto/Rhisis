using Sylver.Network.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Rhisis.World.Game.Structures
{
    public class QuestInfo : IEquatable<QuestInfo>
    {
        public int QuestId { get; }

        public int? DatabaseQuestId { get; }

        public int CharacterId { get; }

        public bool IsFinished { get; set; }

        public bool IsChecked { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime StartTime { get; set; }

        public IDictionary<int, short> Monsters { get; set; }

        public bool IsPatrolDone { get; set; }

        public QuestInfo(int questId, int characterId)
            : this(questId, characterId, default)
        {
        }

        public QuestInfo(int questId, int characterId, int? databaseQuestId)
        {
            this.QuestId = questId;
            this.CharacterId = characterId;
            this.DatabaseQuestId = databaseQuestId;
            this.Monsters = new Dictionary<int, short>();
        }

        public void Serialize(INetPacketStream packet)
        {
            packet.Write<short>(0); // state
            packet.Write<short>(0); // time limit
            packet.Write((short)this.QuestId);

            packet.Write<short>(Monsters.ElementAtOrDefault(0).Value); // monster 1 killed
            packet.Write<short>(Monsters.ElementAtOrDefault(1).Value); // monster 2 killed
            packet.Write<byte>(Convert.ToByte(this.IsPatrolDone)); // patrol done
            packet.Write<byte>(0); // dialog done
        }

        public bool Equals([AllowNull] QuestInfo other)
        {
            return QuestId == other.QuestId && CharacterId == other.CharacterId;
        }
    }
}