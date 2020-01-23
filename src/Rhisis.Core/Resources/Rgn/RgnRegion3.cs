using Rhisis.Core.Structures;

namespace Rhisis.Core.Resources
{
    /// <summary>
    /// "region3" region structure.
    /// </summary>
    public sealed class RgnRegion3 : RgnElement
    {
        public int Index { get; private set; }

        public int Attribute { get; private set; }

        public int MusicId { get; private set; }

        public bool DirectMusic { get; private set; }

        public string Script { get; private set; }

        public string Sound { get; private set; }

        public int TeleportWorldId { get; private set; }

        public Vector3 TeleportPosition { get; private set; }

        public string Key { get; private set; }

        public bool TargetKey { get; private set; }

        public int ItemId { get; private set; }

        public int ItemCount { get; private set; }

        public int MinLevel { get; private set; }

        public int MaxLevel { get; private set; }

        public int QuestId { get; private set; }

        public int QuestFlag { get; private set; }

        public int Job { get; private set; }

        public int Gender { get; private set; }

        public bool CheckParty { get; private set; }

        public bool CheckGuild { get; private set; }

        public bool ChaoKey { get; private set; }

        public RgnRegion3(string[] regionData)
        {
            Type = int.Parse(regionData[1]);
            Index = int.Parse(regionData[2]);
            Position = new Vector3(regionData[3], regionData[4], regionData[5]);
            Attribute = int.Parse(regionData[6].Replace("0x", string.Empty), System.Globalization.NumberStyles.AllowHexSpecifier);
            MusicId = int.Parse(regionData[7]);
            DirectMusic = int.Parse(regionData[8]) == 1;
            Script = regionData[9];
            Sound = regionData[10];
            TeleportWorldId = int.Parse(regionData[11]);
            TeleportPosition = new Vector3(regionData[12], regionData[13], regionData[14]);
            Left = int.Parse(regionData[15]);
            Top = int.Parse(regionData[16]);
            Right = int.Parse(regionData[17]);
            Bottom = int.Parse(regionData[18]);
            Key = regionData[19].Trim('"');
            TargetKey = int.Parse(regionData[20]) == 1;
            ItemId = int.Parse(regionData[21]);
            ItemCount = int.Parse(regionData[22]);
            MinLevel = int.Parse(regionData[23]);
            MaxLevel = int.Parse(regionData[24]);
            QuestId = int.Parse(regionData[25]);
            QuestFlag = int.Parse(regionData[26]);
            Job = int.Parse(regionData[27]);
            Gender = int.Parse(regionData[28]);
            CheckParty = int.Parse(regionData[29]) == 1;
            CheckGuild = int.Parse(regionData[30]) == 1;
            ChaoKey = int.Parse(regionData[31]) == 1;
        }
    }
}
