using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Flowmailer.Helpers.Errors;
using Flowmailer.Helpers.Errors.Models;
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
        private readonly Func<string, IRestClient> _restClientFactory;
        private string _accessToken;

        /// <summary>
        /// Instantiates the Flowmailer API client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="accountId"></param>
        /// <param name="restClientFactory"></param>
        protected FlowmailerClientBase(string clientId, string clientSecret, string accountId, Func<string, IRestClient> restClientFactory = null)
        {
            if (string.IsNullOrWhiteSpace(clientId)) throw new ArgumentNullException(nameof(clientId));
            if (string.IsNullOrWhiteSpace(clientSecret)) throw new ArgumentNullException(nameof(clientSecret));
            if (string.IsNullOrWhiteSpace(accountId)) throw new ArgumentNullException(nameof(accountId));

            _clientId = clientId;
            _clientSecret = clientSecret;
            _accountId = accountId;
            _restClientFactory = restClientFactory;

            try
            {
                _accessToken = GetAccessToken().AccessToken;
            }
            catch (Exception e)
            {
                throw new FlowmailerClientConstructionException(e);
            }
        }

        private OAuthTokenResponse GetAccessToken()
        {
            var restClient = GetRestClient("https://login.flowmailer.net");
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

            if (authResult.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedException();
            }

            var result = JsonConvert.DeserializeObject<OAuthErrorResponse>(authResult.Content);
            if (result == null) return null;

            throw new BadRequestException($"{result.Error}: {result.ErrorDescription}");
        }

        private IRestClient GetRestClient(string baseUrl)
        {
            return _restClientFactory == null ? new RestClient(baseUrl) : _restClientFactory(baseUrl);
        }

        /// <summary>
        /// Runs the provided request, renewing the access token if needed.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The IRestResponse result</returns>
        protected Task<IRestResponse> DoRequestAsync(IRestRequest request)
        {
            return DoRequestAsync(request, response => response);
        }

        /// <summary>
        /// Runs the provided request, renewing the access token if needed.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="onSuccess">Callback that runs on success</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected async Task<T> DoRequestAsync<T>(IRestRequest request, Func<IRestResponse, T> onSuccess)
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

                return onSuccess(responseResult);
            }
            catch (Exception e)
            {
                if (responseResult == null)
                {
                    throw;
                }

                ErrorHandler.ThrowException(responseResult, e);

                return default;
            }
        }

        /// <summary>
        /// Runs the provided request, renewing the access token if needed.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="onSuccess">Callback that runs on success</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>Result of type T</returns>
        protected T DoRequest<T>(IRestRequest request, Func<IRestResponse, T> onSuccess)
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

                return onSuccess(responseResult);
            }
            catch (Exception e)
            {
                if (responseResult == null)
                {
                    throw;
                }

                ErrorHandler.ThrowException(responseResult, e);

                return default;
            }
        }

        /// <summary>
        /// Method for creating a request
        /// </summary>
        /// <param name="method"></param>
        /// <param name="path"></param>
        /// <param name="matrix"></param>
        /// <returns>The request to be executed by a IRestClient</returns>
        protected IRestRequest CreateRequest(Method method, string path, string matrix = "")
        {
            var resource = $"{path}{matrix}";
            var request = new RestRequest(resource, method);

            request.AddHeader("Authorization", $"Bearer {_accessToken}");
            request.AddHeader("Content-Type", "application/vnd.flowmailer.v1.11+json");
            request.AddHeader("Accept", "application/vnd.flowmailer.v1.11+json");
            return request;
        }

        /// <summary>
        /// Takes the response and parses out the Message ID that is returned after a successful POST to 'message/submit'.
        /// </summary>
        /// <param name="responseResult"></param>
        /// <returns>The message ID</returns>
        protected static string GetMessageId(IRestResponse responseResult)
        {
            var locationUri = responseResult?.Headers?.FirstOrDefault(h => h.Name == "Location")?.Value?.ToString() ?? "";
            if (string.IsNullOrEmpty(locationUri) || locationUri.LastIndexOf("/", StringComparison.Ordinal) == -1)
            {
                return string.Empty;
            }

            var messageId = locationUri.Substring(locationUri.LastIndexOf("/", StringComparison.Ordinal) + 1);
            return messageId;
        }

        private IRestClient GetRestClient()
        {
            var restClient = GetRestClient($"https://api.flowmailer.net/{_accountId}");
            restClient.UseNewtonsoftJson();
            return restClient;
        }
    }

    /// <summary>
    /// Exception thrown when trying to fetch Auth token in FlowmailerClientBase constructor.
    /// Inner exception holds the exception that caused the initialization to fail.
    /// </summary>
    public class FlowmailerClientConstructionException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exception">The inner exception that caused the initialization to fail.</param>
        public FlowmailerClientConstructionException(Exception exception) : base("Could not fetch Auth token.", exception)
        {
        }
    }
}