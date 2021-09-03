using System.Threading.Tasks;
using Flowmailer.Models;
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
            return await DoRequestAsync(CreateSendMessageRequest(message));
        }

        /// <summary>
        /// Sends a message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>MessageId as <see cref="string"/></returns>
        public string SendMessage(SubmitMessage message)
        {
            return DoRequest(CreateSendMessageRequest(message));
        }

        ///// <summary>
        ///// Gets message events.
        ///// </summary>
        ///// <param name="message"></param>
        //public async Task GetMessageEvents(SubmitMessage message)
        //{
        //    var sendMessageDo = SendMessageDoAsync(message);
        //    await DoRequestAsync(sendMessageDo);
        //}

        private IRestRequest CreateSendMessageRequest(SubmitMessage message)
        {
            var request = CreateRequest();
            request.AddJsonBody(message);

            return request;
        }

        //private async Task<IRestResponse> GetMessageEventsDo(SubmitMessage message, int numberOfItems = 0)
        //{
        //    var restClient = CreateRequest(out var request);

        //    request.AddJsonBody(message);
        //    request.AddHeader("Range", $"items=:{numberOfItems}");


        //    var response = await restClient.ExecuteAsync(request);
        //    if (response.IsSuccessful)
        //    {
        //        var responseData = JsonConvert.DeserializeObject<List<MessageEvent>>(response.Content);
        //        if (responseData.Count >= 100)
        //        {

        //        }
        //    }

        //    var range = response.Headers.FirstOrDefault(h => h.Name == "Content-Range");



        //    return response;
        //}
    }
}
