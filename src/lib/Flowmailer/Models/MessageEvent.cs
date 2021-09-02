using Newtonsoft.Json;

namespace Flowmailer.Models
{
    /// <summary>
    /// Holder for Message event data
    /// </summary>
    public class MessageEvent
    {
        /// <summary>
        /// base64-encoded data
        /// </summary>
        [JsonProperty("data")]
        public string Data { get; set; }
    }
}