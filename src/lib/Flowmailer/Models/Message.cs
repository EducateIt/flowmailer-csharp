using System.Collections.Generic;

namespace Flowmailer.Models
{
    public class Message
    {
        public object Data { get; set; }
        public string HeaderFromAddress { get; set; }
        public string HeaderFromName { get; set; }
        public string HeaderToName { get; set; }
        public Header[] Headers { get; set; }
        public string MessageType { get; set; }
        public string RecipientAddress { get; set; }
        public string SenderAddress { get; set; }
        public string Subject { get; set; }
        public string Html { get; set; }

        public static class MessageTypes
        {
            public static string EMAIL = "EMAIL";
        }
    }
}