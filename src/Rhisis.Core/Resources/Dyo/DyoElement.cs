using Rhisis.Core.Structures;
using System.IO;

namespace Rhisis.Core.Resources.Dyo
{
    public class DyoElement
    {
        public int ElementType { get; internal set; }

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
            Axis = new Vector3();
            Position = new Vector3();
            Scale = new Vector3();
        }

        public virtual void Read(BinaryReader reader)
        {
            Angle = reader.ReadSingle();
            Axis.X = reader.ReadSingle();
            Axis.Y = reader.ReadSingle();
            Axis.Z = reader.ReadSingle();
            Position.X = reader.ReadSingle() * 4f;
            Position.Y = reader.ReadSingle();
            Position.Z = reader.ReadSingle() * 4f;
            Scale.X = reader.ReadSingle();
            Scale.Y = reader.ReadSingle();
            Scale.Z = reader.ReadSingle();
            Type = reader.ReadInt32();
            Index = reader.ReadInt32();
            Motion = reader.ReadInt32();
            IAInterface = reader.ReadInt32();
            IA2 = reader.ReadInt32();
        }
    }
}
