namespace Rhisis.World.Core.Components
{
    public class HumanComponent : IComponent
    {
        public byte Gender { get; set; }

        public int SkinSetId { get; set; }

        public int HairId { get; set; }

        public uint HairColor { get; set; }

        public int FaceId { get; set; }
    }
}
