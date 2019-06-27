using Rhisis.World.Game.Core.Systems;

namespace Rhisis.World.Systems.Customization.EventArgs
{
    public class ChangeFaceEventArgs : SystemEventArgs
    {
        public uint ObjectId { get; }

        public uint FaceId { get; }

        public bool UseCoupon { get; }

        public ChangeFaceEventArgs(uint objectId, uint faceId, bool bUseCoupon)
        {
            ObjectId = objectId;
            FaceId = faceId;
            UseCoupon = bUseCoupon;
        }

        public override bool GetCheckArguments()
        {
            return ObjectId > 0;
        }
    }
}
