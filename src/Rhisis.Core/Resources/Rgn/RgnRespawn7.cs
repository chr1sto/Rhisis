using Rhisis.Core.Structures;

namespace Rhisis.Core.Resources
{
    /// <summary>
    /// "respawn7" region structure.
    /// </summary>
    public sealed class RgnRespawn7 : RgnElement
    {
        public int Model { get; private set; }

        public int Count { get; private set; }

        public int Time { get; private set; }

        public int AgroNumber { get; private set; }

        public RgnRespawn7(string[] respawnData)
        {
            this.Type = int.Parse(respawnData[1]);
            this.Model = int.Parse(respawnData[2]);
            this.Position = new Vector3(respawnData[3], respawnData[4], respawnData[5]);
            this.Count = int.Parse(respawnData[6]);
            this.Time = int.Parse(respawnData[7]);
            this.AgroNumber = int.Parse(respawnData[8]);
            this.Left = int.Parse(respawnData[9]);
            this.Top = int.Parse(respawnData[10]);
            this.Right = int.Parse(respawnData[11]);
            this.Bottom = int.Parse(respawnData[12]);
        }
    }
}
