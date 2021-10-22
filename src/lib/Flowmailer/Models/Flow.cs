using JetBrains.Annotations;
using Newtonsoft.Json;
#pragma warning disable 1591

namespace Flowmailer.Models
{
    [PublicAPI]
    public class Flow
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}