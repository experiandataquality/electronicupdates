﻿//-----------------------------------------------------------------------
// <copyright file="MetadataApiException.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Runtime.Serialization;

namespace Experian.Qas.Updates.Metadata.WebApi.V2
{
    /// <summary>
    /// Represents error data when an error is returned by the Experian Data Quality Electronic Updates Metadata API.
    /// </summary>
    #if NetStandard2_0
    [Serializable]
    #endif
    public class MetadataApiException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataApiException"/> class.
        /// </summary>
        public MetadataApiException()
            : base("An error was returned by the Electronic Updates Metadata REST API.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataApiException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MetadataApiException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataApiException"/> class with a
        /// specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or <see langword="null"/>
        /// if no inner exception is specified.
        /// </param>
        public MetadataApiException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

#if NetStandard2_0
        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataApiException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="info"/> parameter is <see langword="null"/>.
        /// </exception>
        protected MetadataApiException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}
