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

        public bool[] SetJob { get; private set; } = new bool[(int)DefineJob.MAX_JOB];

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

            this.Version = reader.ReadUInt32();

            if (this.Version == CommonControlVersion1)
            {
                this.Set = reader.ReadUInt32();
                this.SetItem = reader.ReadUInt32();
                this.SetLevel = reader.ReadUInt32();
                this.SetQuestNum = reader.ReadUInt32();
                this.SetFlagNum = reader.ReadUInt32();
                this.SetGender = reader.ReadUInt32();

                for (int i = 0; i < this.SetJob.Length; i++)
                    this.SetJob[i] = reader.ReadInt32() == 1;

                this.SetEndu = reader.ReadUInt32();

                this.MinItemNum = reader.ReadUInt32();
                this.MaxItemNum = reader.ReadUInt32();

                for (int i = 0; i < MaxControlDropItem; i++)
                {
                    this.InsideItemKind[i] = reader.ReadUInt32();
                    this.InsideItemPer[i] = reader.ReadUInt32();
                }

                for (int i = 0; i < MaxControlDropMonster; i++)
                {
                    this.MonsterResistanceKind[i] = reader.ReadUInt32();
                    this.MonsterResistanceNum[i] = reader.ReadUInt32();
                    this.MonsterActionAttack[i] = reader.ReadUInt32();
                }

                this.TrapOperTime = reader.ReadUInt32();
                this.TrapRandomPer = reader.ReadUInt32();
                this.TrapDelay = reader.ReadUInt32();

                for (int i = 0; i < MaxTrap; i++)
                {
                    this.TrapKind[i] = reader.ReadUInt32();
                    this.TrapLevel[i] = reader.ReadUInt32();
                }

                this.LinkControlKey = Encoding.Default.GetString(reader.ReadBytes(MaxKey));
                this.ControlKey = Encoding.Default.GetString(reader.ReadBytes(MaxKey));

                this.SetQuestNum1 = reader.ReadUInt32();
                this.SetFlagNum1 = reader.ReadUInt32();
                this.SetQuestNum2 = reader.ReadUInt32();
                this.SetFlagNum2 = reader.ReadUInt32();
                this.SetItemCount = reader.ReadUInt32();
                this.TeleportWorldId = reader.ReadUInt32();
                this.TeleportX = reader.ReadUInt32();
                this.TeleportY = reader.ReadUInt32();
                this.TeleportZ = reader.ReadUInt32();
            }
            else if (this.Version == CommonControlVersion2)
            {
                this.Set = reader.ReadUInt32();
                this.SetItem = reader.ReadUInt32();
                this.SetLevel = reader.ReadUInt32();
                this.SetQuestNum = reader.ReadUInt32();
                this.SetFlagNum = reader.ReadUInt32();
                this.SetGender = reader.ReadUInt32();

                for (int i = 0; i < this.SetJob.Length; i++)
                    this.SetJob[i] = reader.ReadInt32() == 1;

                this.SetEndu = reader.ReadUInt32();

                this.MinItemNum = reader.ReadUInt32();
                this.MaxItemNum = reader.ReadUInt32();

                for (int i = 0; i < MaxControlDropItem; i++)
                {
                    this.InsideItemKind[i] = reader.ReadUInt32();
                }
                this.InsideItemPer[0] = reader.ReadUInt32();

                this.TrapOperTime = reader.ReadUInt32();
                this.TrapRandomPer = reader.ReadUInt32();
                this.TrapDelay = reader.ReadUInt32();

                for (int i = 0; i < MaxTrap; i++)
                {
                    this.TrapKind[i] = reader.ReadUInt32();
                    this.TrapLevel[i] = reader.ReadUInt32();
                }

                this.LinkControlKey = Encoding.Default.GetString(reader.ReadBytes(MaxKey));
                this.ControlKey = Encoding.Default.GetString(reader.ReadBytes(MaxKey));

                this.SetQuestNum1 = reader.ReadUInt32();
                this.SetFlagNum1 = reader.ReadUInt32();
                this.SetQuestNum2 = reader.ReadUInt32();
                this.SetFlagNum2 = reader.ReadUInt32();
                this.SetItemCount = reader.ReadUInt32();
                this.TeleportWorldId = reader.ReadUInt32();
                this.TeleportX = reader.ReadUInt32();
                this.TeleportY = reader.ReadUInt32();
                this.TeleportZ = reader.ReadUInt32();
            }
            else
            {
                this.Set = this.Version;
                reader.BaseStream.Position += Size - (sizeof(uint) * 10);
                this.SetItem = reader.ReadUInt32();
            }
        }
    }
}
