using Newtonsoft.Json;

namespace Flowmailer.Models
{
    /// <summary>
    /// OAuth error response holder
    /// Described in http://tools.ietf.org/html/rfc6749
    /// </summary>
    public class OAuthErrorResponse
    {
        /// <summary>
        /// Error title
        /// </summary>
        [JsonProperty("error")]
        public string Error { get; set; }

        /// <summary>
        /// Error description
        /// </summary>
        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }

        /// <summary>
        /// Error URI
        /// </summary>
        [JsonProperty("error_uri")]
        public string ErrorUri { get; set; }
    }
}