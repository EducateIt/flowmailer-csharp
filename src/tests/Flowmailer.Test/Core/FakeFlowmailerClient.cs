using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flowmailer.Models;
using RestSharp;

namespace Flowmailer.Test.Core
{
    public class FakeFlowmailerClient : FlowmailerClientBase, IFlowmailerClient
    {
        public FakeFlowmailerClient(string clientId, string clientSecret, string accountId, Func<string, IRestClient> restClientFactory = null) : base(clientId, clientSecret, accountId, restClientFactory)
        {
        }

        public Task<string> SendMessageAsync(SubmitMessage message)
        {
            return Task.FromResult(Guid.NewGuid().ToString("N"));
        }

        public string SendMessage(SubmitMessage message)
        {
            return Guid.NewGuid().ToString("N");
        }

        public Task<List<MessageEvent>> GetMessageEventsAsync(DateTime from, DateTime to)
        {
            return Task.FromResult(new List<MessageEvent>
            {
                new MessageEvent
                {
                    Received = GetRandom.DateTime(from, to)
                }
            });
        }

        public Task<Message> GetMessageAsync(string messageId)
        {
            return Task.FromResult(new Message
            {
                Id = Guid.NewGuid().ToString("N")
            });
        }
    }
}