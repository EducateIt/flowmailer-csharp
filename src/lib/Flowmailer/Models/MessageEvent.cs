using System;
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

        /// <summary>
        /// Message event ID
        /// </summary>
        [JsonProperty("id")]
        public string id { get; set; }
        public string messageId { get; set; }
        public string type { get; set; }
        public DateTime received { get; set; }
        public DateTime inserted { get; set; }
        public string snippet { get; set; }
        public string mta { get; set; }
        public string referer { get; set; }
        public string remoteAddr { get; set; }
        public string userAgentString { get; set; }
        public string deviceCategory { get; set; }
        public string userAgent { get; set; }
        public string userAgentType { get; set; }
        public string userAgentVersion { get; set; }
        public string userAgentDisplayName { get; set; }
        public string operatingSystem { get; set; }
        public string operatingSystemVersion { get; set; }
        public string linkTarget { get; set; }
        public string linkName { get; set; }
    }
}