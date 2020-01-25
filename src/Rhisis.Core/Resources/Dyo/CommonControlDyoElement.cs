using Rhisis.Core.Data;
using System.IO;
using System.Text;

namespace Rhisis.Core.Resources.Dyo
{
    public sealed class CommonControlDyoElement : DyoElement
    {
        private const uint CommonControlVersion1 = 0x80000000;
        private const uint CommonControlVersion2 = 0x90000000;
        private const int MaxControlDropItem = 4;
        private const int MaxControlDropMonster = 3;
        private const int MaxTrap = 3;
        private const int MaxKey = 64;

        /// <summary>
        /// Defines the size of the data structure.
        /// </summary>
        private const int Size = 432;

        public uint Version { get; private set; }

        public uint Set { get; private set; }

        public uint SetItem { get; private set; }

        public uint SetLevel { get; private set; }

        public uint SetQuestNum { get; private set; }

        public uint SetFlagNum { get; private set; }

        public uint SetGender { get; private set; }

        public bool[] SetJob { get; private set; } = new bool[(int)DefineJob.JobMax.MAX_JOB];

        public uint SetEndu { get; private set; }

        public uint MinItemNum { get; private set; }

        public uint MaxItemNum { get; private set; }

        public uint[] InsideItemKind { get; private set; } = new uint[MaxControlDropItem];

        public uint[] InsideItemPer { get; private set; } = new uint[MaxControlDropItem];

        public uint[] MonsterResistanceKind { get; private set; } = new uint[MaxControlDropMonster];

        public uint[] MonsterResistanceNum { get; private set; } = new uint[MaxControlDropMonster];

        public uint[] MonsterActionAttack { get; private set; } = new uint[MaxControlDropMonster];

        public uint TrapOperTime { get; private set; }

        public uint TrapRandomPer { get; private set; }

        public uint TrapDelay { get; private set; }

        public uint[] TrapKind { get; private set; } = new uint[MaxTrap];

        public uint[] TrapLevel { get; private set; } = new uint[MaxTrap];

        public string LinkControlKey { get; private set; }

        public string ControlKey { get; private set; }

        public uint SetQuestNum1 { get; private set; }

        public uint SetFlagNum1 { get; private set; }

        public uint SetQuestNum2 { get; private set; }

        public uint SetFlagNum2 { get; private set; }

        public uint SetItemCount { get; private set; }

        public uint TeleportWorldId { get; private set; }

        public uint TeleportX { get; private set; }

        public uint TeleportY { get; private set; }

        public uint TeleportZ { get; private set; }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);

            Version = reader.ReadUInt32();

            if (Version == CommonControlVersion1)
            {
                Set = reader.ReadUInt32();
                SetItem = reader.ReadUInt32();
                SetLevel = reader.ReadUInt32();
                SetQuestNum = reader.ReadUInt32();
                SetFlagNum = reader.ReadUInt32();
                SetGender = reader.ReadUInt32();

                for (int i = 0; i < SetJob.Length; i++)
                    SetJob[i] = reader.ReadInt32() == 1;

                SetEndu = reader.ReadUInt32();

                MinItemNum = reader.ReadUInt32();
                MaxItemNum = reader.ReadUInt32();

                for (int i = 0; i < MaxControlDropItem; i++)
                {
                    InsideItemKind[i] = reader.ReadUInt32();
                    InsideItemPer[i] = reader.ReadUInt32();
                }

                for (int i = 0; i < MaxControlDropMonster; i++)
                {
                    MonsterResistanceKind[i] = reader.ReadUInt32();
                    MonsterResistanceNum[i] = reader.ReadUInt32();
                    MonsterActionAttack[i] = reader.ReadUInt32();
                }

                TrapOperTime = reader.ReadUInt32();
                TrapRandomPer = reader.ReadUInt32();
                TrapDelay = reader.ReadUInt32();

                for (int i = 0; i < MaxTrap; i++)
                {
                    TrapKind[i] = reader.ReadUInt32();
                    TrapLevel[i] = reader.ReadUInt32();
                }

                LinkControlKey = Encoding.Default.GetString(reader.ReadBytes(MaxKey));
                ControlKey = Encoding.Default.GetString(reader.ReadBytes(MaxKey));

                SetQuestNum1 = reader.ReadUInt32();
                SetFlagNum1 = reader.ReadUInt32();
                SetQuestNum2 = reader.ReadUInt32();
                SetFlagNum2 = reader.ReadUInt32();
                SetItemCount = reader.ReadUInt32();
                TeleportWorldId = reader.ReadUInt32();
                TeleportX = reader.ReadUInt32();
                TeleportY = reader.ReadUInt32();
                TeleportZ = reader.ReadUInt32();
            }
            else if (Version == CommonControlVersion2)
            {
                Set = reader.ReadUInt32();
                SetItem = reader.ReadUInt32();
                SetLevel = reader.ReadUInt32();
                SetQuestNum = reader.ReadUInt32();
                SetFlagNum = reader.ReadUInt32();
                SetGender = reader.ReadUInt32();

                for (int i = 0; i < SetJob.Length; i++)
                    SetJob[i] = reader.ReadInt32() == 1;

                SetEndu = reader.ReadUInt32();

                MinItemNum = reader.ReadUInt32();
                MaxItemNum = reader.ReadUInt32();

                for (int i = 0; i < MaxControlDropItem; i++)
                {
                    InsideItemKind[i] = reader.ReadUInt32();
                }
                InsideItemPer[0] = reader.ReadUInt32();

                TrapOperTime = reader.ReadUInt32();
                TrapRandomPer = reader.ReadUInt32();
                TrapDelay = reader.ReadUInt32();

                for (int i = 0; i < MaxTrap; i++)
                {
                    TrapKind[i] = reader.ReadUInt32();
                    TrapLevel[i] = reader.ReadUInt32();
                }

                LinkControlKey = Encoding.Default.GetString(reader.ReadBytes(MaxKey));
                ControlKey = Encoding.Default.GetString(reader.ReadBytes(MaxKey));

                SetQuestNum1 = reader.ReadUInt32();
                SetFlagNum1 = reader.ReadUInt32();
                SetQuestNum2 = reader.ReadUInt32();
                SetFlagNum2 = reader.ReadUInt32();
                SetItemCount = reader.ReadUInt32();
                TeleportWorldId = reader.ReadUInt32();
                TeleportX = reader.ReadUInt32();
                TeleportY = reader.ReadUInt32();
                TeleportZ = reader.ReadUInt32();
            }
            else
            {
                Set = Version;
                reader.BaseStream.Position += Size - (sizeof(uint) * 10);
                SetItem = reader.ReadUInt32();
            }
        }
    }
}
