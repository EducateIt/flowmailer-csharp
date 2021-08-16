using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Flowmailer.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace Flowmailer
{
    /// <summary>
    /// The Flowmailer Client responsible for issuing requests to the Flowmailer API.
    /// </summary>
    public class FlowmailerClient
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _accountId;
        private string _accessToken;

        /// <summary>
        /// Instantiates the Flowmailer API client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="accountId"></param>
        public FlowmailerClient(string clientId, string clientSecret, string accountId)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _accountId = accountId;

            GetAccessToken();
        }

        private void GetAccessToken()
        {
            var restClient = new RestClient("https://login.flowmailer.net");
            // restClient.Proxy = new WebProxy("127.0.0.1:8888");
            var request = new RestRequest("oauth/token", Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("client_id", _clientId);
            request.AddParameter("client_secret", _clientSecret);
            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("scope", "api");

            IRestResponse authResult;

            try
            {
                authResult = restClient.Execute(request);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            var content = authResult.Content;
            try
            {
                _accessToken = JsonConvert.DeserializeAnonymousType(content, new { access_token = "", expires_in = 0, token_type = "", scope = "" })?.access_token ?? "";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        /// <summary>
        /// Sends a message.
        /// </summary>
        /// <param name="message"></param>
        public async Task SendMessage(Message message)
        {
            var restClient = new RestClient($"https://api.flowmailer.net/{_accountId}");
            restClient.UseNewtonsoftJson();
            var request = new RestRequest("messages/submit", Method.POST);
            // restClient.Proxy = new WebProxy("127.0.0.1:8888");
            request.AddHeader("Authorization", $"Bearer {_accessToken}");
            request.AddHeader("Content-Type", "application/vnd.flowmailer.v1.11+json");
            request.AddHeader("Accept", "application/vnd.flowmailer.v1.11+json");
            request.AddJsonBody(message);

            IRestResponse authResult;

            try
            {
                authResult = await restClient.ExecuteAsync(request);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            var content = authResult.Content;
            Console.WriteLine(content);
        }
    }
}
