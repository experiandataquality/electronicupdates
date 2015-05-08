//-----------------------------------------------------------------------
// <copyright file="DataFile.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Diagnostics;
using System.Runtime.Serialization;

namespace Experian.Qas.Updates.Metadata.WebApi.V1
{
    /// <summary>
    /// A class representing a single data file.
    /// </summary>
    [DataContract(Namespace = "", Name = "DataFile")]
    [DebuggerDisplay("{FileName} {MD5Hash}")]
    public class DataFile
    {
        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        [DataMember(Name = "FileName")]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the size of the file in bytes.
        /// </summary>
        [DataMember(Name = "Size")]
        public long Size { get; set; }

        /// <summary>
        /// Gets or sets the MD5 hash of the file.
        /// </summary>
        [DataMember(Name = "Md5Hash")]
        public string MD5Hash { get; set; }
    }
}