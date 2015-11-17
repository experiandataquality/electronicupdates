//-----------------------------------------------------------------------
// <copyright file="FileDownloadReply.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Newtonsoft.Json;

namespace Experian.Qas.Updates.Metadata.WebApi.V1
{
    /// <summary>
    /// A class representing the details returned to the client when requesting to download a file.
    /// </summary>
    public class FileDownloadReply
    {
        /// <summary>
        /// Gets or sets the URI to download the file from.
        /// </summary>
        [JsonProperty("DownloadUri")]
        public string DownloadUri { get; set; }
    }
}
