using System;

namespace Flowmailer.Helpers.Errors.Models
{
    /// <summary>
    /// Represents errors with status code 4xx.
    /// </summary>
    public class RequestErrorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestErrorException"/> class.
        /// </summary>
        public RequestErrorException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestErrorException"/> class with a specified error message.
        /// </summary>
        /// <param name="message"> The error message that explains the reason for the exception.</param>
        public RequestErrorException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestErrorException"/> class with a specified error and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference  if no inner exception is specified.</param>
        public RequestErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}