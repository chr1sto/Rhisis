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
            Type = int.Parse(respawnData[1]);
            Model = int.Parse(respawnData[2]);
            Position = new Vector3(respawnData[3], respawnData[4], respawnData[5]);
            Count = int.Parse(respawnData[6]);
            Time = int.Parse(respawnData[7]);
            AgroNumber = int.Parse(respawnData[8]);
            Left = int.Parse(respawnData[9]);
            Top = int.Parse(respawnData[10]);
            Right = int.Parse(respawnData[11]);
            Bottom = int.Parse(respawnData[12]);
        }
    }
}
