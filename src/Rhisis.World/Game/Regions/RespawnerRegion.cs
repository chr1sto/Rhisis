namespace Rhisis.World.Game.Regions
{
    public class RespawnerRegion : Region
    {
        /// <summary>
        /// Gets the region respawn time.
        /// </summary>
        public int RespawnTime { get; }

        /// <summary>
        /// Gets the respawner's object type.
        /// </summary>
        public int ObjectType { get; }

        /// <summary>
        /// Gets the respawner's mover id.
        /// </summary>
        public int MoverId { get; }

        /// <summary>
        /// Gets the respawner's mover count.
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Creates a new <see cref="RespawnerRegion"/> instance.
        /// </summary>
        /// <param name="left">X coordinate (X Top Left corner)</param>
        /// <param name="top">Z coordinate (Z top left corner)</param>
        /// <param name="right">Width of the region</param>
        /// <param name="bottom">Height of the region</param>
        /// <param name="respawnTime">Respawn time</param>
        /// <param name="objectType">Respawner object type</param>
        /// <param name="moverId">Respawner mover id</param>
        /// <param name="count">Respawner mover count</param>
        public RespawnerRegion(int left, int top, int right, int bottom, int respawnTime, int objectType, int moverId, int count)
            : base(left, top, right, bottom)
        {
            this.RespawnTime = respawnTime;
            this.ObjectType = objectType;
            this.MoverId = moverId;
            this.Count = count;
        }
    }
}
