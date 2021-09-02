using Newtonsoft.Json;

namespace Flowmailer.Models
{
    /// <summary>
    /// OAuth response holder
    /// </summary>
    public class OAuthTokenResponse
    {

        /// <summary>
        /// The requested access token that is to be passed with every call to the Flowmailer API
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// The number of seconds this token is valid
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Type of the returned token, only "bearer" is supported
        /// </summary>
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// The scope of the token
        /// Only "api" is supported
        /// </summary>
        [JsonProperty("scope")]
        public string Scope { get; set; }
    }


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