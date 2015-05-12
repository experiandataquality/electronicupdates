//-----------------------------------------------------------------------
// <copyright file="PackageGroup.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Experian.Qas.Updates.Metadata.WebApi.V1
{
    /// <summary>
    /// A class representing a single vintage of data.
    /// </summary>
    [DataContract(Namespace = "", Name = "PackageGroup")]
    [DebuggerDisplay("{PackageGroupCode} {Vintage}")]
    public class PackageGroup
    {
        /// <summary>
        /// Gets or sets the package group code.
        /// </summary>
        [DataMember(Name = "PackageGroupCode")]
        public string PackageGroupCode { get; set; }

        /// <summary>
        /// Gets or sets the dataset vintage.
        /// </summary>
        [DataMember(Name = "Vintage")]
        public string Vintage { get; set; }

        /// <summary>
        /// Gets or sets the individual packages of this group.
        /// </summary>
        [DataMember(Name = "Packages")]
        public List<Package> Packages { get; set; }
    }
}