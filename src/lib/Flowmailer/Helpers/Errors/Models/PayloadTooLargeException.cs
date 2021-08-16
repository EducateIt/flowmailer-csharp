﻿using System;

namespace Flowmailer.Helpers.Errors.Models
{
    /// <summary>
    /// Represents errors with status code 413
    /// </summary>
    public class PayloadTooLargeException : RequestErrorException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PayloadTooLargeException"/> class.
        /// </summary>
        public PayloadTooLargeException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PayloadTooLargeException"/> class with a specified error.
        /// </summary>
        /// <param name="message"> The error message that explains the reason for the exception.</param>
        public PayloadTooLargeException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PayloadTooLargeException"/> class with a specified error and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference  if no inner exception is specified.</param>
        public PayloadTooLargeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}