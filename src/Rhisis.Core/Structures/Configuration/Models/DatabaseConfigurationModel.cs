using Newtonsoft.Json;

namespace Rhisis.Core.Structures.Configuration.Models
{
    public class DatabaseConfigurationModel
    {
        [JsonProperty(PropertyName = nameof(DatabaseConfiguration))]
        public DatabaseConfiguration DatabaseConfiguration { get; set; }
    }
}
