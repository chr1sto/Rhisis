using Rhisis.World.Game.Core.Systems;

namespace Rhisis.World.Systems.Customization.EventArgs
{
    public class SetHairEventArgs : SystemEventArgs
    {
        public byte HairId { get; }

        public byte R { get; }

        public byte G { get; }

        public byte B { get; }

        public bool UseCoupon { get; }

        public SetHairEventArgs(byte hairId, byte r, byte g, byte b, bool bUseCoupon)
        {
            HairId = hairId;
            R = r;
            G = g;
            B = b;
            UseCoupon = bUseCoupon;
        }

        public override bool CheckArguments()
        {
            return true;
        }
    }
}
