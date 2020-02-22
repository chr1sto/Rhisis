using Rhisis.Core.Data;
using Rhisis.Network.Packets;
using Rhisis.World.Game.Structures;
using Sylver.Network.Data;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Game.Components
{
    public class SkillTreeComponent : IPacketSerializer
    {
        /// <summary>
        /// Gets the skills information.
        /// </summary>
        public IEnumerable<SkillInfo> Skills { get; set; }

        /// <summary>
        /// Gets the skill based on the given id.
        /// </summary>
        /// <param name="skillId">Skill id.</param>
        /// <returns></returns>
        public SkillInfo GetSkill(int skillId) => Skills.FirstOrDefault(x => x.SkillId == skillId);

        /// <summary>
        /// Gets the skill based on the given skill index.
        /// </summary>
        /// <param name="skillIndex">Skill index.</param>
        /// <returns></returns>
        public SkillInfo GetSkillByIndex(int skillIndex) => Skills.ElementAt(skillIndex);

        /// <summary>
        /// Sets the skill level of the given skill id.
        /// </summary>
        /// <param name="skillId">Skill id.</param>
        /// <param name="level">Skill level.</param>
        public void SetSkillLevel(int skillId, int level)
        {
            SkillInfo skill = GetSkill(skillId);

            if (skill != null)
            {
                skill.Level = level;
            }
        }

        /// <summary>
        /// Check if the given skill id is exists in the skill tree.
        /// </summary>
        /// <param name="skillId">Skill id.</param>
        /// <returns>True if the skill exists; false otherwise.</returns>
        public bool HasSkill(int skillId) => Skills.Any(x => x.SkillId == skillId);

        /// <inheritdoc />
        public void Serialize(INetPacketStream packet)
        {
            int skillCount = Skills.Count();
            int otherSkillCount = (int)DefineJob.JobMax.MAX_SKILLS - skillCount;

            for (int i = 0; i < skillCount; i++)
            {
                Skills.ElementAt(i).Serialize(packet);
            }

            for (int i = 0; i < otherSkillCount; i++)
            {
                packet.Write(-1);
                packet.Write(0);
            }
        }
    }
}
