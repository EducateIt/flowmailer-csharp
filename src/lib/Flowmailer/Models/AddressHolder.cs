using JetBrains.Annotations;
using Newtonsoft.Json;
#pragma warning disable 1591

namespace Flowmailer.Models
{
    [PublicAPI]
    public class AddressHolder
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
    }
}