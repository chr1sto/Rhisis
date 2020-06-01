using System;

namespace Rhisis.Core.Resources
{
    /// <summary>
    /// Defines a <see cref="WldFile"/> informations data structure.
    /// </summary>
    public readonly struct WldFileInformations : IEquatable<WldFileInformations>
    {
        /// <summary>
        /// Gets the world file width value.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Gets the world file length value.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Gets the MPU (meters per unit) value.
        /// </summary>
        public int MPU { get; }

        /// <summary>
        /// Gets a value that indicates if the world is indoor or not.
        /// </summary>
        public bool Indoor { get; }

        /// <summary>
        /// Gets a value that indicates if we can fly in the world.
        /// </summary>
        public bool Fly { get;}

        /// <summary>
        /// Gets the revival map id.
        /// </summary>
        public int RevivalMapId { get; }

        /// <summary>
        /// Gets the revival key.
        /// </summary>
        public string RevivalKey { get; }

        /// <summary>
        /// Creates a new <see cref="WldFileInformations"/> structure.
        /// </summary>
        /// <param name="width">World width.</param>
        /// <param name="length">World length.</param>
        /// <param name="mpu">World metters per unit.</param>
        /// <param name="isIndoor">Flag that indicates if the world is indoor. (eg. dungeon)</param>
        /// <param name="canFly">Flag that indicates if the world authorizes flying objects.</param>
        /// <param name="revivalMapId">World revival map id.</param>
        /// <param name="revivalKey">World revival key.</param>
        public WldFileInformations(int width, int length, int mpu, bool isIndoor, bool canFly, int revivalMapId, string revivalKey)
        {
            Width = width;
            Length = length;
            MPU = mpu;
            Indoor = isIndoor;
            Fly = canFly;
            RevivalMapId = revivalMapId;
            RevivalKey = revivalKey;
        }

        /// <summary>
        /// Compares the current instance with another <see cref="WldFileInformations"/>.
        /// </summary>
        /// <param name="other">Other <see cref="WldFileInformations"/>.</param>
        /// <returns>True if the same; false otherwise.</returns>
        public bool Equals(WldFileInformations other) 
            => (Width, Length, MPU, Indoor, Fly, RevivalMapId, RevivalKey) ==
               (other.Width, other.Length, other.MPU, other.Indoor, other.Fly, other.RevivalMapId, other.RevivalKey);
    }
}
