using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.World.Game.Entities;
using System;

namespace Rhisis.World.Game.Chat
{
    public class TestCommand
    {
        struct SKILL : IEquatable<SKILL>
        {
            public uint dwSkill;
            public uint dwLevel;

            public bool Equals(SKILL other)
            {
                return dwSkill == other.dwSkill && dwLevel == other.dwLevel;
            }
        }

        [ChatCommand(".test", Rhisis.Core.Common.AuthorityType.Administrator)]
        public static void OnTest(IPlayerEntity player, string[] parameters)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(player.Id, SnapshotType.SETEXPERIENCE);

                packet.Write((long)0);
                packet.Write((ushort)5);
                packet.Write(5);
                packet.Write(10);
                packet.Write((long)0);
                packet.Write((ushort)0);

                player.Connection.Send(packet);
            }

            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(player.Id, SnapshotType.SET_JOB_SKILL);

                const int MAX_JOB_SKILL = 45;

                packet.Write(0); // JOB ID
                
                for(int i = 0; i < MAX_JOB_SKILL; i++)
                {
                    SKILL skill;
                    if (i == 0)
                    {
                        skill.dwSkill = 1;
                        skill.dwLevel = 3;
                    }
                    else if(i == 1)
                    {
                        skill.dwSkill = 2;
                        skill.dwLevel = 1;
                    }
                    else
                    {
                        skill.dwSkill = 0xffffffff;
                        skill.dwLevel = 0;
                    }

                    packet.Write(skill.dwSkill);
                    packet.Write(skill.dwLevel);
                }

                for (int i = 0; i < 32; i++)
                    packet.Write(0);

                player.Connection.Send(packet);
            }


        }
    }
}