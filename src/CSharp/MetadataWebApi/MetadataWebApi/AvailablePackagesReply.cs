//-----------------------------------------------------------------------
// <copyright file="AvailablePackagesReply.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Experian.Qas.Updates.Metadata.WebApi.V1
{
    /// <summary>
    /// A class representing the data required by the client to download files.
    /// </summary>
    public class AvailablePackagesReply
    {
        /// <summary>
        /// Gets or sets the instances of the packages.
        /// </summary>
        [JsonProperty("PackageGroups")]
        public List<PackageGroup> PackageGroups { get; set; }
    }
}
