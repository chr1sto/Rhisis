using Newtonsoft.Json;

namespace Rhisis.Core.Structures.Configuration.Models
{
    public class DatabaseConfigurationModel
    {
        [JsonProperty(PropertyName = ConfigurationConstants.DatabaseConfiguration)]
        public DatabaseConfiguration DatabaseConfiguration { get; set; }
    }
}
