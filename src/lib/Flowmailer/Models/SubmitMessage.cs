using System.Collections.Generic;

namespace Flowmailer.Models
{
    /// <summary>
    /// SubmitMessage class for Flowmailer API
    /// </summary>
    public class SubmitMessage
    {
        /// <summary>
        /// Collection of variables to send along the message
        /// </summary>
        public Dictionary<string, string> Data { get; set; }
        /// <summary>
        /// Header value for from address
        /// </summary>
        public string HeaderFromAddress { get; set; }
        /// <summary>
        /// Header value for from name
        /// </summary>
        public string HeaderFromName { get; set; }
        /// <summary>
        /// Header value for to name
        /// </summary>
        public string HeaderToName { get; set; }
        /// <summary>
        /// Additional headers to send in the message
        /// </summary>
        public Header[] Headers { get; set; }
        /// <summary>
        /// The type of message, <see cref="MessageTypes"/>
        /// </summary>
        public string MessageType { get; set; }
        /// <summary>
        /// The recipient address
        /// </summary>
        public string RecipientAddress { get; set; }
        /// <summary>
        /// The sender address
        /// </summary>
        public string SenderAddress { get; set; }
        /// <summary>
        /// Subject of the message
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// The raw html message
        /// </summary>
        public string Html { get; set; }

        /// <summary>
        /// Freely configurable value that can be used to select a flow or one of its variants.
        /// </summary>
        public string FlowSelector { get; set; }
    }
}
