﻿using System;

namespace RedCell.Research
{
    /// <summary>
    /// Class ResearchException.
    /// </summary>
    public class ResearchException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResearchException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public ResearchException(string message = null, Exception innerException = null) : base(message, innerException)
        {

        }
    }
}
