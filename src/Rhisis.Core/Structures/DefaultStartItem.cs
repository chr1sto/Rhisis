using System.Runtime.Serialization;

namespace Rhisis.Core.Structures
{
    [DataContract]
    public class DefaultStartItem
    {
        [DataMember(Name = "startWeapon")]
        public int StartWeapon { get; set; }

        [DataMember(Name = "startSuit")]
        public int StartSuit { get; set; }

        [DataMember(Name = "startHand")]
        public int StartHand { get; set; }

        [DataMember(Name = "startShoes")]
        public int StartShoes { get; set; }

        [DataMember(Name = "startHat")]
        public int StartHat { get; set; }

        public DefaultStartItem()
        {
            this.StartWeapon = -1;
            this.StartSuit = -1;
            this.StartHand = -1;
            this.StartShoes = -1;
            this.StartHat = -1;
        }
    }
}
