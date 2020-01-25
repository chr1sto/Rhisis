using Rhisis.Core.Structures.Game.Quests;
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

        public IQuestScript Script { get; }

        public QuestStateType State { get; set; }

        public bool IsFinished { get; set; }

        public bool IsChecked { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime StartTime { get; set; }

        public IDictionary<int, short> Monsters { get; set; }

        public bool IsPatrolDone { get; set; }

        public QuestInfo(int questId, int characterId, IQuestScript script)
            : this(questId, characterId, script, default)
        {
        }

        public QuestInfo(int questId, int characterId, IQuestScript script, int? databaseQuestId)
        {
            QuestId = questId;
            CharacterId = characterId;
            Script = script;
            DatabaseQuestId = databaseQuestId;
            Monsters = new Dictionary<int, short>();
        }

        public void Serialize(INetPacketStream packet)
        {
            packet.Write<short>((short)State); // state
            packet.Write<short>(0); // time limit
            packet.Write((short)QuestId);

            packet.Write<short>(Monsters?.ElementAtOrDefault(0).Value ?? 0); // monster 1 killed
            packet.Write<short>(Monsters?.ElementAtOrDefault(1).Value ?? 0); // monster 2 killed
            packet.Write<byte>(Convert.ToByte(IsPatrolDone)); // patrol done
            packet.Write<byte>(0); // dialog done
        }

        public bool Equals([AllowNull] QuestInfo other)
        {
            if (other is null)
                return false;

            return QuestId == other.QuestId && CharacterId == other.CharacterId;
        }

        public override int GetHashCode()
        {
            return (QuestId, CharacterId).GetHashCode();
        }
    }
}