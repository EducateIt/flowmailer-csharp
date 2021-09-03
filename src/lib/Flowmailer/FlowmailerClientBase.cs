using System;
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
    /// Base-class for the Flowmailer Client responsible for issuing requests to the Flowmailer API.
    /// Contains methods for creating and making requests
    /// </summary>
    public abstract class FlowmailerClientBase
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
        protected FlowmailerClientBase(string clientId, string clientSecret, string accountId)
        {
            if (string.IsNullOrWhiteSpace(clientId)) throw new ArgumentNullException(nameof(clientId));
            if (string.IsNullOrWhiteSpace(clientSecret)) throw new ArgumentNullException(nameof(clientSecret));
            if (string.IsNullOrWhiteSpace(accountId)) throw new ArgumentNullException(nameof(accountId));

            _clientId = clientId;
            _clientSecret = clientSecret;
            _accountId = accountId;

            _accessToken = GetAccessToken().AccessToken;
        }

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

        protected async Task<string> DoRequestAsync(IRestRequest request)
        {
            var restClient = GetRestClient();
            IRestResponse responseResult = null;
            try
            {
                var keepTrying = true;
                var counter = 0;

                while (keepTrying && counter < 10)
                {
                    responseResult = await restClient.ExecuteAsync(request);

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

                return GetMessageId(responseResult);
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

        protected string DoRequest(IRestRequest request)
        {
            var restClient = GetRestClient();
            IRestResponse responseResult = null;
            try
            {
                var keepTrying = true;
                var counter = 0;

                while (keepTrying && counter < 10)
                {

                    responseResult = restClient.Execute(request);

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

                return GetMessageId(responseResult);
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

        protected IRestRequest CreateRequest()
        {
            var request = new RestRequest("messages/submit", Method.POST);
            request.AddHeader("Authorization", $"Bearer {_accessToken}");
            request.AddHeader("Content-Type", "application/vnd.flowmailer.v1.11+json");
            request.AddHeader("Accept", "application/vnd.flowmailer.v1.11+json");
            return request;
        }

        private static string GetMessageId(IRestResponse responseResult)
        {
            var locationUri = responseResult?.Headers?.FirstOrDefault(h => h.Name == "Location")?.Value?.ToString() ?? "";
            if (string.IsNullOrEmpty(locationUri) || locationUri.LastIndexOf("/", StringComparison.Ordinal) == -1)
            {
                return string.Empty;
            }

            var messageId = locationUri.Substring(locationUri.LastIndexOf("/", StringComparison.Ordinal) + 1);
            return messageId;
        }

        private RestClient GetRestClient()
        {
            var restClient = new RestClient($"https://api.flowmailer.net/{_accountId}");
            restClient.UseNewtonsoftJson();
            return restClient;
        }
    }
}