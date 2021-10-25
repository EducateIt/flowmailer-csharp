using System;
using Newtonsoft.Json;
#pragma warning disable 1591

namespace Flowmailer.Models
{
    public class MessageEvent
    {
        [JsonProperty("data")]
        public string Data { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("messageId")]
        public string MessageId { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("received")]
        public DateTime Received { get; set; }
        [JsonProperty("inserted")]
        public DateTime Inserted { get; set; }
        [JsonProperty("snippet")]
        public string Snippet { get; set; }
        [JsonProperty("mta")]
        public string Mta { get; set; }
        [JsonProperty("referer")]
        public string Referer { get; set; }
        [JsonProperty("remoteAddr")]
        public string RemoteAddr { get; set; }
        [JsonProperty("userAgentString")]
        public string UserAgentString { get; set; }
        [JsonProperty("deviceCategory")]
        public string DeviceCategory { get; set; }
        [JsonProperty("userAgent")]
        public string UserAgent { get; set; }
        [JsonProperty("userAgentType")]
        public string UserAgentType { get; set; }
        [JsonProperty("userAgentVersion")]
        public string UserAgentVersion { get; set; }
        [JsonProperty("userAgentDisplayName")]
        public string UserAgentDisplayName { get; set; }
        [JsonProperty("operatingSystem")]
        public string OperatingSystem { get; set; }
        [JsonProperty("operatingSystemVersion")]
        public string OperatingSystemVersion { get; set; }
        [JsonProperty("linkTarget")]
        public string LinkTarget { get; set; }
        [JsonProperty("linkName")]
        public string LinkName { get; set; }
    }
}