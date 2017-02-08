//-----------------------------------------------------------------------
// <copyright file="GetDownloadUriRequest.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Runtime.Serialization;

namespace Experian.Qas.Updates.Metadata.WebApi.V2
{
    /// <summary>
    /// A class representing a request to obtain the download URI for a data file.
    /// </summary>
    [DataContract(Name = "GetFileDownload", Namespace = "")]
    public class GetDownloadUriRequest
    {
        /// <summary>
        /// Gets or sets the data for the file to request the download URL for.
        /// </summary>
        [DataMember(Name = "fileDownloadRequest", IsRequired = true, Order = 1)]
        public FileDownloadRequest RequestData { get; set; }
    }
}
