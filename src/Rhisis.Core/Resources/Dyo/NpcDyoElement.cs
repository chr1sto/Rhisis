using System.IO;
using System.Text;

namespace Rhisis.Core.Resources.Dyo
{
    public class NpcDyoElement : DyoElement
    {
        public string Name { get; private set; }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);

            reader.ReadBytes(64); // Korean name
            reader.ReadBytes(32); // dialog name
            this.Name = Encoding.GetEncoding(0).GetString(reader.ReadBytes(32));
            this.Name = this.Name.Substring(0, this.Name.IndexOf('\0'));
            reader.ReadInt32(); // ??
            reader.ReadInt32(); // ??
        }
    }
}
