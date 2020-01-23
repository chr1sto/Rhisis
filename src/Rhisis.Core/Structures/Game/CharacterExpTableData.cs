using System;

namespace Rhisis.Core.Structures.Game
{
    public readonly struct CharacterExpTableData : IEquatable<CharacterExpTableData>
    {
        /// <summary>
        /// Gets the level.
        /// </summary>
        public int Level { get; }

        /// <summary>
        /// Gets the experience required for the current level.
        /// </summary>
        public long Exp { get; }

        /// <summary>
        /// Gets another kind of experience.
        /// </summary>
        /// <remarks>
        /// This seems not to be used in official files.
        /// </remarks>
        public long Pxp { get; }

        /// <summary>
        /// Gets the number of statistics points given for the current level.
        /// </summary>
        public long Gp { get; }

        /// <summary>
        /// Gets the experience limit of the current level.
        /// </summary>
        public long LimExp { get; }

        /// <summary>
        /// Creates a new <see cref="CharacterExpTableData"/> instance.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="experience"></param>
        /// <param name="pxp"></param>
        /// <param name="gp"></param>
        /// <param name="limitExperience"></param>
        public CharacterExpTableData(int level, long experience, long pxp, long gp, long limitExperience)
        {
            Level = level;
            Exp = experience;
            Pxp = pxp;
            Gp = gp;
            LimExp = limitExperience;
        }

        /// <summary>
        /// Compares two instances of <see cref="CharacterExpTableData"/>.
        /// </summary>
        /// <param name="other">Other <see cref="CharacterExpTableData"/>.</param>
        /// <returns>True if the same; false otherwise.</returns>
        public bool Equals(CharacterExpTableData other) 
            => (Level, Exp, Pxp, Gp, LimExp) == (other.Level, other.Exp, other.Pxp, other.Gp, other.LimExp);
    }
}
