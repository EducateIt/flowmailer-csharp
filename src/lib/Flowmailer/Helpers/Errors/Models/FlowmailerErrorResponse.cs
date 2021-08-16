namespace Flowmailer.Helpers.Errors.Models
{
    /// <summary>
    /// Base class for the json error response
    /// </summary>
    public class FlowmailerErrorResponse
    {
        /// <summary>
        /// Gets or sets the error Status Code
        /// </summary>
        public int ErrorHttpStatusCode { get; set; }

        /// <summary>
        /// Gets or sets the error Reason Phrase
        /// </summary>
        public string ErrorReasonPhrase { get; set; }

        /// <summary>
        /// Gets or sets the Flowmailer error message
        /// </summary>
        public string FlowmailerErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the field that has the error
        /// </summary>
        public string FieldWithError { get; set; }

        /// <summary>
        /// Gets or sets the object name that has the error
        /// </summary>
        public string ObjectName { get; set; }

        /// <summary>
        /// Gets or sets the code value
        /// </summary>
        public string ErrorCode { get; set; }
    }
}