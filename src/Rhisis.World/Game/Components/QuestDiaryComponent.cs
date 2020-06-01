using Rhisis.World.Game.Structures;
using Sylver.Network.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Game.Components
{
    public class QuestDiaryComponent : ICollection<QuestInfo>
    {
        private readonly List<QuestInfo> _quests;

        /// <summary>
        /// Gets the active quests.
        /// </summary>
        public IEnumerable<QuestInfo> ActiveQuests => _quests.Where(x => !x.IsFinished);

        /// <summary>
        /// Gets the completed quests.
        /// </summary>
        public IEnumerable<QuestInfo> CompletedQuests => _quests.Where(x => x.IsFinished);

        /// <summary>
        /// Gets the active checked quests.
        /// </summary>
        public IEnumerable<QuestInfo> CheckedQuests => ActiveQuests.Where(x => x.IsChecked);

        /// <summary>
        /// Gets the total amount of quests in the diary.
        /// </summary>
        public int Count => _quests.Count;

        /// <summary>
        /// Gets a value that indicates if the diary is in readonly mode.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Creates a new empty <see cref="QuestDiaryComponent"/> instance.
        /// </summary>
        public QuestDiaryComponent()
        {
            _quests = new List<QuestInfo>();
        }

        /// <summary>
        /// Creates a new <see cref="QuestDiaryComponent"/> instance from a collection of <see cref="QuestInfo"/>.
        /// </summary>
        /// <param name="quests">Quest information.</param>
        public QuestDiaryComponent(IEnumerable<QuestInfo> quests)
        {
            _quests = quests.ToList();
        }

        /// <summary>
        /// Checks if the quest exists in the diary.
        /// </summary>
        /// <param name="questId">Quest Id.</param>
        /// <returns>True if the quest exists, false otherwise.</returns>
        public bool HasQuest(int questId) => _quests.Any(x => x.QuestId == questId);

        /// <summary>
        /// Gets the active quest based on the given id.
        /// </summary>
        /// <param name="questId">Quest id.</param>
        /// <returns><see cref="QuestInfo"/>.</returns>
        public QuestInfo GetActiveQuest(int questId) => ActiveQuests.FirstOrDefault(x => x.QuestId == questId);

        /// <summary>
        /// Serialize the quest diary.
        /// </summary>
        /// <param name="packet">Packet stream.</param>
        public void Serialize(INetPacketStream packet)
        {
            packet.Write((byte)ActiveQuests.Count());
            foreach (QuestInfo quest in ActiveQuests)
            {
                quest.Serialize(packet);
            }

            packet.Write((byte)CompletedQuests.Count());
            foreach (QuestInfo quest in CompletedQuests)
            {
                packet.Write((short)quest.QuestId);
            }

            packet.Write((byte)CheckedQuests.Count());
            foreach (QuestInfo quest in CheckedQuests)
            {
                packet.Write((short)quest.QuestId);
            }
        }

        /// <summary>
        /// Adds a new quest to the diary.
        /// </summary>
        /// <param name="item"></param>
        public void Add(QuestInfo item)
        {
            if (Contains(item))
            {
                throw new InvalidOperationException($"Quest '{item.QuestId}' for player with id '{item.CharacterId}' already exists.");
            }

            _quests.Add(item);
        }

        /// <summary>
        /// Clears the quest diary.
        /// </summary>
        public void Clear() => throw new NotImplementedException();

        /// <summary>
        /// Checks if the quest diary contains the given <see cref="QuestInfo"/>.
        /// </summary>
        /// <param name="item">Quest information.</param>
        /// <returns>True if the quest exist, false otherwise.</returns>
        public bool Contains(QuestInfo item)
        {
            return _quests.Contains(item);
        }

        /// <summary>
        /// Copy the quests into an array.
        /// </summary>
        /// <param name="array">Destination array.</param>
        /// <param name="arrayIndex">Array index.</param>
        public void CopyTo(QuestInfo[] array, int arrayIndex) => throw new NotImplementedException();

        /// <summary>
        /// Removes a quest from the diary.
        /// </summary>
        /// <param name="item">Quest to remove.</param>
        /// <returns></returns>
        public bool Remove(QuestInfo item)
        {
            item.IsDeleted = true;

            return true;
        }

        /// <summary>
        /// Gets the quest diary enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<QuestInfo> GetEnumerator() => _quests.GetEnumerator();

        /// <summary>
        /// Gets the quest diary enumerator.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() => _quests.GetEnumerator();
    }
}
