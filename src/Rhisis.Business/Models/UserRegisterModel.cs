using System.Runtime.Serialization;

namespace Rhisis.Business.Models
{
    [DataContract]
    public class UserRegisterModel
    {
        [DataMember(Name = "username")]
        public string Username { get; set; }

        [DataMember(Name = "password")]
        public string Password { get; set; }

        [DataMember(Name = "passwordConfirmation")]
        public string PasswordConfirmation { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }
    }
}
