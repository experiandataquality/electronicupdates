//-----------------------------------------------------------------------
// <copyright file="Package.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Experian.Qas.Updates.Metadata.WebApi.V2
{
    /// <summary>
    /// A class representing an instance of a package.
    /// </summary>
    [DataContract(Namespace = "", Name = "Package")]
    [DebuggerDisplay("{PackageCode}")]
    public class Package
    {
        /// <summary>
        /// Gets or sets the package code.
        /// </summary>
        [DataMember(Name = "PackageCode")]
        public string PackageCode { get; set; }

        /// <summary>
        /// Gets or sets the list of the files associated with this package.
        /// </summary>
        [DataMember(Name = "Files")]
        public List<DataFile> Files { get; set; }
    }
}
