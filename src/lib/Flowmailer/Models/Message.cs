using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
#pragma warning disable 1591

namespace Flowmailer.Models
{
    [PublicAPI]
    public class Message
    {
        [JsonProperty("submitted")]
        public DateTime Submitted { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }
        [JsonProperty("messageIdHeader")]
        public string MessageIdHeader { get; set; }
        [JsonProperty("messageType")]
        public string MessageType { get; set; }
        [JsonProperty("source")]
        public Source Source { get; set; }
        [JsonProperty("flow")]
        public Flow Flow { get; set; }
        [JsonProperty("senderAddress")]
        public string SenderAddress { get; set; }
        [JsonProperty("recipientAddress")]
        public string RecipientAddress { get; set; }
        [JsonProperty("backendStart")]
        public DateTime BackendStart { get; set; }
        [JsonProperty("backendDone")]
        public DateTime? BackendDone { get; set; }
        [JsonProperty("headersIn")]
        public Header[] HeadersIn { get; set; }
        [JsonProperty("headersOut")]
        public Header[] HeadersOut { get; set; }
        [JsonProperty("onlineLink")]
        public string OnlineLink { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("subject")]
        public string Subject { get; set; }
        [JsonProperty("from")]
        public string From { get; set; }
        [JsonProperty("events")]
        public MessageEvent[] Events { get; set; }
        [JsonProperty("messageDetailsLink")]
        public string MessageDetailsLink { get; set; }
        [JsonProperty("fromAddress")]
        public AddressHolder FromAddress { get; set; }
        [JsonProperty("toAddressList")]
        public AddressHolder[] ToAddressList { get; set; }
    }
}
