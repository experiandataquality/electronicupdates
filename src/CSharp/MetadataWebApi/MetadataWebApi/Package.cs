//-----------------------------------------------------------------------
// <copyright file="Package.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Experian.Qas.Updates.Metadata.WebApi.V1
{
    /// <summary>
    /// A class representing an instance of a package.
    /// </summary>
    [DebuggerDisplay("{PackageCode}")]
    public class Package
    {
        /// <summary>
        /// Gets or sets the package code.
        /// </summary>
        [JsonProperty("PackageCode")]
        public string PackageCode { get; set; }

        /// <summary>
        /// Gets or sets the list of the files associated with this package.
        /// </summary>
        [JsonProperty("Files")]
        public List<DataFile> Files { get; set; }
    }
}
