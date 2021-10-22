using System;
using RestSharp;

namespace Flowmailer.Test.Fakes
{
    public class FakeFlowmailerClient : FlowmailerClientBase
    {
        public FakeFlowmailerClient(string clientId, string clientSecret, string accountId, Func<string, IRestClient> restClientFactory = null) : base(clientId, clientSecret, accountId, restClientFactory)
        {
        }
    }
}