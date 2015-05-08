//-----------------------------------------------------------------------
// <copyright file="FileDownloadRequest.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Runtime.Serialization;

namespace Experian.Qas.Updates.Metadata.WebApi.V1
{
    /// <summary>
    /// A class representing details of a request to download a file.
    /// </summary>
    [DataContract(Namespace = "", Name = "FileDownloadRequest")]
    public class FileDownloadRequest
    {
        /// <summary>
        /// Gets or sets the name of the file requested to be downloaded.
        /// </summary>
        [DataMember(Name = "FileName", IsRequired = true)]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the MD5 of the file requested to be downloaded.
        /// </summary>
        [DataMember(Name = "FileMd5Hash", IsRequired = true)]
        public string FileMD5Hash { get; set; }

        /// <summary>
        /// Gets or sets the byte to start downloading from, if any.
        /// </summary>
        [DataMember(Name = "StartAtByte", IsRequired = false)]
        public long? StartAtByte { get; set; }

        /// <summary>
        /// Gets or sets the byte to end downloading at, if any.
        /// </summary>
        [DataMember(Name = "EndAtByte", IsRequired = false)]
        public long? EndAtByte { get; set; }
    }
}