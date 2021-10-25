using System;
using Flowmailer.Helpers.Errors.Models;
using RestSharp;

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
        /// <param name="response"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static void ThrowException(IRestResponse response, Exception exception)
        {
            var errorMessage = response.ErrorMessage;

            var errorStatusCode = (int)response.StatusCode;

            switch (errorStatusCode)
            {
                // 400 - BAD REQUEST
                case 400:
                    throw new BadRequestException(errorMessage, exception);

                // 401 - UNAUTHORIZED
                case 401:
                    throw new UnauthorizedException(errorMessage, exception);

                // 403 - FORBIDDEN
                case 403:
                    throw new ForbiddenException(errorMessage, exception);

                // 404 - NOT FOUND
                case 404:
                    throw new NotFoundException(errorMessage, exception);

                case 500:
                    throw new InternalServerErrorException(errorMessage, exception);

            }

            // 4xx - Error with the request
            if (errorStatusCode >= 400 && errorStatusCode < 500)
            {
                throw new RequestErrorException(errorMessage, exception);
            }

            // 5xx - Error made by Flowmailer
            if (errorStatusCode >= 500)
            {
                throw new FlowmailerInternalException(errorMessage, exception);
            }

            throw new BadRequestException(errorMessage, exception);
        }
    }
}