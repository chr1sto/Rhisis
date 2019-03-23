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
            this.Type = int.Parse(regionData[1]);
            this.Index = int.Parse(regionData[2]);
            this.Position = new Vector3(regionData[3], regionData[4], regionData[5]);
            this.Attribute = int.Parse(regionData[6].Replace("0x", string.Empty), System.Globalization.NumberStyles.AllowHexSpecifier);
            this.MusicId = int.Parse(regionData[7]);
            this.DirectMusic = int.Parse(regionData[8]) == 1;
            this.Script = regionData[9];
            this.Sound = regionData[10];
            this.TeleportWorldId = int.Parse(regionData[11]);
            this.TeleportPosition = new Vector3(regionData[12], regionData[13], regionData[14]);
            this.Left = int.Parse(regionData[15]);
            this.Top = int.Parse(regionData[16]);
            this.Right = int.Parse(regionData[17]);
            this.Bottom = int.Parse(regionData[18]);
            this.Key = regionData[19].Trim('"');
            this.TargetKey = int.Parse(regionData[20]) == 1;
            this.ItemId = int.Parse(regionData[21]);
            this.ItemCount = int.Parse(regionData[22]);
            this.MinLevel = int.Parse(regionData[23]);
            this.MaxLevel = int.Parse(regionData[24]);
            this.QuestId = int.Parse(regionData[25]);
            this.QuestFlag = int.Parse(regionData[26]);
            this.Job = int.Parse(regionData[27]);
            this.Gender = int.Parse(regionData[28]);
            this.CheckParty = int.Parse(regionData[29]) == 1;
            this.CheckGuild = int.Parse(regionData[30]) == 1;
            this.ChaoKey = int.Parse(regionData[31]) == 1;
        }
    }
}
