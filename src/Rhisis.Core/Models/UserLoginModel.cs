using System.Runtime.Serialization;

namespace Rhisis.Core.Models
{
    [DataContract]
    public class UserLoginModel
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        [DataMember]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [DataMember]
        public string Password { get; set; }
    }
}
