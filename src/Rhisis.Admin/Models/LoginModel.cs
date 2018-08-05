using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Rhisis.Admin.Models
{
    [DataContract]
    public class LoginModel
    {
        [Required]
        [DataMember(Name = "username")]
        public string Username { get; set; }

        [Required]
        [DataMember(Name = "password")]
        public string Password { get; set; }
    }
}
