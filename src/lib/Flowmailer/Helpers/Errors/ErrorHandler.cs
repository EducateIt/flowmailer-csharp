using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Flowmailer.Helpers.Errors.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Flowmailer.Helpers.Errors
{
    /// <summary>
    /// Error handler for requests.
    /// </summary>
    public class ErrorHandler
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task ThrowException(HttpResponseMessage message)
        {
            var errorMessage = await GetErrorMessage(message).ConfigureAwait(false);

            var errorStatusCode = (int)message.StatusCode;

            switch (errorStatusCode)
            {
                // 400 - BAD REQUEST
                case 400:
                    throw new BadRequestException(errorMessage);

                // 401 - UNAUTHORIZED
                case 401:
                    throw new UnauthorizedException(errorMessage);

                // 403 - FORBIDDEN
                case 403:
                    throw new ForbiddenException(errorMessage);

                // 404 - NOT FOUND
                case 404:
                    throw new NotFoundException(errorMessage);

                // 405 - METHOD NOT ALLOWED
                case 405:
                    throw new MethodNotAllowedException(errorMessage);

                // 413 - PAYLOAD TOO LARGE
                case 413:
                    throw new PayloadTooLargeException(errorMessage);

                // 415 - UNSUPPORTED MEDIA TYPE
                case 415:
                    throw new UnsupportedMediaTypeException(errorMessage);

                // 429 - TOO MANY REQUESTS
                case 429:
                    throw new TooManyRequestsException(errorMessage);

                // 500 - SERVER UNAVAILABLE
                case 500:
                    throw new InternalServerErrorException(errorMessage);

                // 503 - SERVICE NOT AVAILABLE
                case 503:
                    throw new ServiceNotAvailableException(errorMessage);
            }

            // 4xx - Error with the request
            if (errorStatusCode >= 400 && errorStatusCode < 500)
            {
                throw new RequestErrorException(errorMessage);
            }

            // 5xx - Error made by Flowmailer
            if (errorStatusCode >= 500)
            {
                throw new FlowmailerInternalException(errorMessage);
            }

            throw new BadRequestException(errorMessage);
        }

        /// <summary>
        /// Get error based on Response from Flowmailer API
        /// </summary>
        /// <param name="message">Response Message from API</param>
        /// <returns>Return string with the error Status Code and the Message</returns>
        private static async Task<string> GetErrorMessage(HttpResponseMessage message)
        {
            var errorStatusCode = (int)message.StatusCode;
            var errorReasonPhrase = message.ReasonPhrase;

            var errorResponses = new List<FlowmailerErrorResponse>();

            if (message.Content == null) return JsonConvert.SerializeObject(errorResponses);

            var responseContent = await message.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!string.IsNullOrEmpty(responseContent))
            {
                try
                {
                    // Check for the presence of property called 'errors'
                    var jObject = JObject.Parse(responseContent);
                    var errorsArray = (JArray)jObject["allErrors"];
                    if (errorsArray != null && errorsArray.Count > 0)
                    {
                        foreach (var error in errorsArray)
                        {


                            // Get the first error message
                            var codeValue = (error["code"] ?? "").Value<string>();
                            var errorValue = (error["defaultMessage"] ?? "").Value<string>();
                            var fieldValue = (error["field"] ?? "").Value<string>();
                            var objectNameValue = (error["objectName"] ?? "").Value<string>();

                            errorResponses.Add(new FlowmailerErrorResponse
                            {
                                ErrorHttpStatusCode = errorStatusCode,
                                ErrorReasonPhrase = errorReasonPhrase,
                                ErrorCode = codeValue,
                                FlowmailerErrorMessage = errorValue,
                                FieldWithError = fieldValue,
                                ObjectName = objectNameValue
                            });
                        }
                    }
                }
                catch
                {
                    // Intentionally ignore parsing errors to return default error message
                }
            }

            return JsonConvert.SerializeObject(errorResponses);
        }
    }
}