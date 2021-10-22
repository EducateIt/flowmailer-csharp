using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Flowmailer.Models;
using Newtonsoft.Json;
using RestSharp;

namespace Flowmailer
{
    /// <summary>
    /// The Flowmailer Client responsible for issuing requests to the Flowmailer API.
    /// </summary>
    public sealed class FlowmailerClient : FlowmailerClientBase
    {
        /// <summary>
        /// Instantiates the Flowmailer API client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="accountId"></param>
        /// <exception cref="FlowmailerClientConstructionException"></exception>
        public FlowmailerClient(string clientId, string clientSecret, string accountId) : base(clientId, clientSecret, accountId)
        {
        }

        /// <summary>
        /// Sends a message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>MessageId as <see cref="string"/></returns>
        public async Task<string> SendMessageAsync(SubmitMessage message)
        {
            return await DoRequestAsync(CreateSendMessageRequest(message), GetMessageId);
        }

        /// <summary>
        /// Sends a message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>MessageId as <see cref="string"/></returns>
        public string SendMessage(SubmitMessage message)
        {
            return DoRequest(CreateSendMessageRequest(message), GetMessageId);
        }

        /// <summary>
        /// Gets message events.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public async Task<List<MessageEvent>> GetMessageEventsAsync(DateTime from, DateTime to)
        {
            return await GetMessageEventsDoAsync(from, to);
        }

        private IRestRequest CreateSendMessageRequest(SubmitMessage message)
        {
            var request = CreateRequest(Method.POST, $"messages/submit");

            request.AddJsonBody(message);

            return request;
        }

        private async Task<List<MessageEvent>> GetMessageEventsDoAsync(DateTime from, DateTime to, int numberOfItems = 300)
        {
            var result = new List<MessageEvent>();

            var dateRangeMatrix = $";receivedrange={To8601(from)},{To8601(to)}";

            await MessageEventsDo(numberOfItems, dateRangeMatrix, result);

            return result;
        }

        private async Task MessageEventsDo(int numberOfItems, string receivedRangeMatrix, List<MessageEvent> result, string nextRange = null)
        {
            var request = CreateRequest(Method.GET, $"message_events", receivedRangeMatrix);

            if (nextRange == null)
            {
                request.AddHeader("range", $"items=0-{numberOfItems}");
            }
            else
            {
                request.AddHeader("range", nextRange);
            }

            try
            {
                var response = await DoRequestAsync(request);

                if (string.IsNullOrEmpty(response.Content))
                {
                    return;
                }

                var messageEvents = JsonConvert.DeserializeObject<List<MessageEvent>>(response.Content);
                if (messageEvents == null)
                {
                    return;
                }

                result.AddRange(messageEvents);

                var contentRange = (string)response.Headers.FirstOrDefault(h => h.Name == "Content-Range")?.Value;
                if (string.IsNullOrEmpty(contentRange))
                {
                    return;
                }

                if (!TryGetNextRangeFromContentRangeHeader(contentRange, out nextRange)) return;

                await MessageEventsDo(numberOfItems, receivedRangeMatrix, result, nextRange);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        private static bool TryGetNextRangeFromContentRangeHeader(string contentRange, out string nextRange)
        {
            nextRange = "";

            contentRange = contentRange.Replace("items ", "");

            var indexOfSlash = contentRange.IndexOf("/", StringComparison.InvariantCulture);
            var totalNumOfItems = int.Parse(contentRange.Substring(indexOfSlash + 1));
            var theRangeString = contentRange.Substring(0, indexOfSlash);

            var theRange = theRangeString.Split(new[] { '-' }, StringSplitOptions.None);
            var toItem = int.Parse(theRange[1]);

            if (totalNumOfItems <= toItem)
            {
                return false;
            }

            nextRange = $"items={toItem + 1}-{totalNumOfItems}";

            return true;
        }

        private string To8601(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }
    }
}
