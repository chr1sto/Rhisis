namespace Rhisis.World.Game.Regions
{
    public class RespawnerRegion : Region
    {
        /// <summary>
        /// Gets the region respawn time.
        /// </summary>
        public int RespawnTime { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left">X coordinate (X Top Left corner)</param>
        /// <param name="top">Z coordinate (Z top left corner)</param>
        /// <param name="right">Width of the region</param>
        /// <param name="bottom">Height of the region</param>
        /// <param name="respawnTime">Respawn time</param>
        public RespawnerRegion(int left, int top, int right, int bottom, int respawnTime)
            : base(left, top, right, bottom)
        {
            this.RespawnTime = respawnTime;
        }
    }
}
