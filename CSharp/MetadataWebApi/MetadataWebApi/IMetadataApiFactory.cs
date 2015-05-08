//-----------------------------------------------------------------------
// <copyright file="IMetadataApiFactory.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Experian.Qas.Updates.Metadata.WebApi.V1
{
    /// <summary>
    /// Defines a factory method for creating instances of <see cref="IMetadataApi"/>.
    /// </summary>
    public interface IMetadataApiFactory
    {
        /// <summary>
        /// Creates a new instance of <see cref="IMetadataApi"/>.
        /// </summary>
        /// <returns>
        /// The created instance of <see cref="IMetadataApi"/>.
        /// </returns>
        IMetadataApi CreateMetadataApi();
    }
}