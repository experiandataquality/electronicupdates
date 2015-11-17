//-----------------------------------------------------------------------
// <copyright file="PackageGroup.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Experian.Qas.Updates.Metadata.WebApi.V1
{
    /// <summary>
    /// A class representing a single vintage of data.
    /// </summary>
    [DebuggerDisplay("{PackageGroupCode} {Vintage}")]
    public class PackageGroup
    {
        /// <summary>
        /// Gets or sets the package group code.
        /// </summary>
        [JsonProperty("PackageGroupCode")]
        public string PackageGroupCode { get; set; }

        /// <summary>
        /// Gets or sets the dataset vintage.
        /// </summary>
        [JsonProperty("Vintage")]
        public string Vintage { get; set; }

        /// <summary>
        /// Gets or sets the individual packages of this group.
        /// </summary>
        [JsonProperty("Packages")]
        public List<Package> Packages { get; set; }
    }
}
