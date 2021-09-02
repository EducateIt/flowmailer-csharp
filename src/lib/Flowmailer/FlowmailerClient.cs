using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Flowmailer.Helpers.Errors;
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
            if (string.IsNullOrWhiteSpace(clientId)) throw new ArgumentNullException(nameof(clientId));
            if (string.IsNullOrWhiteSpace(clientSecret)) throw new ArgumentNullException(nameof(clientSecret));
            if (string.IsNullOrWhiteSpace(accountId)) throw new ArgumentNullException(nameof(accountId));

            _clientId = clientId;
            _clientSecret = clientSecret;
            _accountId = accountId;

            _accessToken = GetAccessToken().AccessToken;
        }

        /// <summary>
        /// Sends a message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>MessageId as <see cref="string"/></returns>
        public async Task<string> SendMessageAsync(SubmitMessage message)
        {
            return await DoRequestAsync(SendMessageDoAsync(message));
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

        private OAuthTokenResponse GetAccessToken()
        {
            var restClient = new RestClient("https://login.flowmailer.net");
            var request = new RestRequest("oauth/token");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Accept", "application/json");
            request.AddParameter("client_id", $"{_clientId}");
            request.AddParameter("client_secret", _clientSecret);
            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("scope", "api");

            IRestResponse authResult = null;

            try
            {
                authResult = restClient.Post(request);
            }
            catch (Exception e)
            {
                if (authResult == null)
                {
                    throw;
                }

                ErrorHandler.ThrowException(authResult, e);
            }

            if (authResult.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<OAuthTokenResponse>(authResult.Content);
            }

            ErrorHandler.ThrowException(authResult, authResult.ErrorException);

            return null;
        }

        private async Task<string> DoRequestAsync(Task<IRestResponse> func)
        {
            IRestResponse responseResult = null;
            try
            {
                var keepTrying = true;
                var counter = 0;

                while (keepTrying && counter < 10)
                {
                    responseResult = await func;

                    if (responseResult.IsSuccessful)
                    {
                        break;
                    }

                    switch (responseResult.StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            var error = JsonConvert.DeserializeObject<OAuthErrorResponse>(responseResult.Content);
                            if ((error?.Error ?? "") == "invalid_token")
                            {
                                _accessToken = GetAccessToken().AccessToken;
                                continue;
                            }

                            ErrorHandler.ThrowException(responseResult, responseResult.ErrorException);
                            break;
                        default:
                            keepTrying = false;
                            ErrorHandler.ThrowException(responseResult, responseResult.ErrorException);
                            break;
                    }

                    counter++;
                }

                var locationUri = responseResult?.Headers?.FirstOrDefault(h => h.Name == "Location")?.Value?.ToString() ?? "";
                if (string.IsNullOrEmpty(locationUri))
                {
                    return string.Empty;
                }

                var messageId = locationUri.Substring(locationUri.LastIndexOf("/", StringComparison.Ordinal) + 1);
                return messageId;
            }
            catch (Exception e)
            {
                if (responseResult == null)
                {
                    throw;
                }

                ErrorHandler.ThrowException(responseResult, e);

                return string.Empty;
            }
        }

        private async Task<IRestResponse> SendMessageDoAsync(SubmitMessage message)
        {
            var restClient = CreateRequest(out var request);
            request.AddJsonBody(message);

            return await restClient.ExecuteAsync(request);
        }

        private IRestResponse SendMessageDo(SubmitMessage message)
        {
            var restClient = CreateRequest(out var request);
            request.AddJsonBody(message);

            return restClient.Execute(request);
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

        private RestClient CreateRequest(out RestRequest request)
        {
            var restClient = new RestClient($"https://api.flowmailer.net/{_accountId}");
            restClient.UseNewtonsoftJson();
            request = new RestRequest("messages/submit", Method.POST);
            request.AddHeader("Authorization", $"Bearer {_accessToken}");
            request.AddHeader("Content-Type", "application/vnd.flowmailer.v1.11+json");
            request.AddHeader("Accept", "application/vnd.flowmailer.v1.11+json");
            return restClient;
        }
    }
}
