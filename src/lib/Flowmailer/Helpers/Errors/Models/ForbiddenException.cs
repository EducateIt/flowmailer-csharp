﻿using System;

namespace Flowmailer.Helpers.Errors.Models
{
    /// <summary>
    /// Represents errors with status code 403
    /// </summary>
    public class ForbiddenException : RequestErrorException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class.
        /// </summary>
        public ForbiddenException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class with a specified error.
        /// </summary>
        /// <param name="message"> The error message that explains the reason for the exception.</param>
        public ForbiddenException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class with a specified error and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference  if no inner exception is specified.</param>
        public ForbiddenException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}