//-----------------------------------------------------------------------
// <copyright file="GetDownloadUriRequest.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Newtonsoft.Json;

namespace Experian.Qas.Updates.Metadata.WebApi.V1
{
    /// <summary>
    /// A class representing a request to obtain the download URI for a data file.
    /// </summary>
    public class GetDownloadUriRequest
    {
        /// <summary>
        /// Gets or sets the credentials for authenticating with the web service.
        /// </summary>
        [JsonProperty("usernamePassword")]
        public UserNamePassword Credentials { get; set; }

        /// <summary>
        /// Gets or sets the data for the file to request the download URL for.
        /// </summary>
        [JsonProperty("fileDownloadRequest")]
        public FileDownloadRequest RequestData { get; set; }
    }
}
