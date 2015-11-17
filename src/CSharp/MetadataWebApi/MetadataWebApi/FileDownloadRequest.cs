//-----------------------------------------------------------------------
// <copyright file="FileDownloadRequest.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Newtonsoft.Json;

namespace Experian.Qas.Updates.Metadata.WebApi.V1
{
    /// <summary>
    /// A class representing details of a request to download a file.
    /// </summary>
    public class FileDownloadRequest
    {
        /// <summary>
        /// Gets or sets the name of the file requested to be downloaded.
        /// </summary>
        [JsonProperty("FileName")]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the MD5 of the file requested to be downloaded.
        /// </summary>
        [JsonProperty("FileMd5Hash")]
        public string FileMD5Hash { get; set; }

        /// <summary>
        /// Gets or sets the byte to start downloading from, if any.
        /// </summary>
        [JsonProperty("StartAtByte")]
        public long? StartAtByte { get; set; }

        /// <summary>
        /// Gets or sets the byte to end downloading at, if any.
        /// </summary>
        [JsonProperty("EndAtByte")]
        public long? EndAtByte { get; set; }
    }
}
