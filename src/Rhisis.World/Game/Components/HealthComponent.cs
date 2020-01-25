namespace Rhisis.World.Game.Components
{
    public class HealthComponent
    {
        /// <summary>
        /// Gets or sets the base object's Hit Points.
        /// </summary>
        public int Hp { get; set; }

        /// <summary>
        /// Gets or sets the base object's Mana Points.
        /// </summary>
        public int Mp { get; set; }

        /// <summary>
        /// Gets or sets the base object's Fatigue Points.
        /// </summary>
        public int Fp { get; set; }

        /// <summary>
        /// Gets a value that indicates if the object is dead.
        /// </summary>
        public bool IsDead => Hp <= 0;
    }
}
