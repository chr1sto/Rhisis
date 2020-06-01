using System.IO;
using System.Text;

namespace Rhisis.Core.Resources.Dyo
{
    /// <summary>
    /// Represents a NPC in a dyo file.
    /// </summary>
    public class NpcDyoElement : DyoElement
    {
        /// <summary>
        /// Gets the Npc Korean name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the Npc dialog name.
        /// </summary>
        public string DialogName { get; private set; }

        /// <summary>
        /// Gets the Npc key.
        /// </summary>
        public string CharacterKey { get; private set; }

        /// <summary>
        /// Gets the Npc belligerence.
        /// </summary>
        public int Belligerence { get; set; }

        /// <summary>
        /// Gets the Npc extra flags.
        /// </summary>
        public int ExtraFlag { get; set; }

        /// <summary>
        /// Reads the NPC informations.
        /// </summary>
        /// <param name="reader"></param>
        public override void Read(BinaryReader reader)
        {
            base.Read(reader);

            Name = ConvertToString(reader.ReadBytes(64));
            DialogName = ConvertToString(reader.ReadBytes(32));
            CharacterKey = ConvertToString(reader.ReadBytes(32));
            Belligerence = reader.ReadInt32();
            ExtraFlag = reader.ReadInt32();
        }

        /// <summary>
        /// Converts a buffer into a string.
        /// </summary>
        /// <remarks>
        /// It trims the converted string if finds a '\0'.
        /// </remarks>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private string ConvertToString(byte[] buffer)
        {
            string convertedString = Encoding.Default.GetString(buffer);

            return convertedString.Substring(0, convertedString.IndexOf('\0'));
        }
    }
}
