using Rhisis.Core.Structures;
using System.IO;

namespace Rhisis.Core.Resources.Dyo
{
    public class DyoElement
    {
        public float Angle { get; private set; }

        public Vector3 Axis { get; private set; }

        public Vector3 Position { get; private set; }

        public Vector3 Scale { get; private set; }

        public int Type { get; private set; }

        public int Index { get; private set; }

        public int Motion { get; private set; }

        public int IAInterface { get; private set; }

        public int IA2 { get; private set; }

        public DyoElement()
        {
            this.Axis = new Vector3();
            this.Position = new Vector3();
            this.Scale = new Vector3();
        }

        public virtual void Read(BinaryReader reader)
        {
            this.Angle = reader.ReadSingle();
            this.Axis.X = reader.ReadSingle();
            this.Axis.Y = reader.ReadSingle();
            this.Axis.Z = reader.ReadSingle();
            this.Position.X = reader.ReadSingle() * 4f;
            this.Position.Y = reader.ReadSingle();
            this.Position.Z = reader.ReadSingle() * 4f;
            this.Scale.X = reader.ReadSingle();
            this.Scale.Y = reader.ReadSingle();
            this.Scale.Z = reader.ReadSingle();
            this.Type = reader.ReadInt32();
            this.Index = reader.ReadInt32();
            this.Motion = reader.ReadInt32();
            this.IAInterface = reader.ReadInt32();
            this.IA2 = reader.ReadInt32();
        }
    }
}
