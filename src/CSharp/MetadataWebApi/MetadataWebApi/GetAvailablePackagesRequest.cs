//--------------------------------------------------------------------------
// <copyright file="GetAvailablePackagesRequest.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//--------------------------------------------------------------------------

using Newtonsoft.Json;

namespace Experian.Qas.Updates.Metadata.WebApi.V1
{
    /// <summary>
    /// A class representing a request for the available packages.
    /// </summary>
    public class GetAvailablePackagesRequest
    {
        /// <summary>
        /// Gets or sets the credentials for authenticating with the web service.
        /// </summary>
        [JsonProperty("usernamePassword")]
        public UserNamePassword Credentials { get; set; }
    }
}
