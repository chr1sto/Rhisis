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
