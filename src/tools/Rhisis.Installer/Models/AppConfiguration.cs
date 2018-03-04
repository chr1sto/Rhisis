using System.Runtime.Serialization;

namespace Rhisis.Installer.Models
{
    [DataContract]
    public class AppConfiguration
    {
        [DataMember(Name = "culture")]
        public string Culture { get; set; }
    }
}
