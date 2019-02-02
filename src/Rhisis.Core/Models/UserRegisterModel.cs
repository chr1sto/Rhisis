using System.Runtime.Serialization;

namespace Rhisis.Core.Models
{
    [DataContract]
    public class UserRegisterModel
    {
        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string PasswordConfirmation { get; set; }
    }
}
