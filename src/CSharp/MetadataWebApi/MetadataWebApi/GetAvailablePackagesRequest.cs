//--------------------------------------------------------------------------
// <copyright file="GetAvailablePackagesRequest.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//--------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace Experian.Qas.Updates.Metadata.WebApi.V1
{
    /// <summary>
    /// A class representing a request for the available packages.
    /// </summary>
    [DataContract(Name = "GetAvailablePackages", Namespace = "")]
    public class GetAvailablePackagesRequest
    {
        /// <summary>
        /// Gets or sets the credentials for authenticating with the web service.
        /// </summary>
        [DataMember(Name = "usernamePassword", IsRequired = true)]
        public UserNamePassword Credentials { get; set; }
    }
}