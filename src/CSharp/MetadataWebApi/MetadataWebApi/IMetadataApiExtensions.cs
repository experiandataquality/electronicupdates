//-----------------------------------------------------------------------
// <copyright file="IMetadataApiExtensions.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Experian.Qas.Updates.Metadata.WebApi.V2
{
    /// <summary>
    /// A class containing extension methods for the <see cref="IMetadataApi"/> class. This class cannot be inherited.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class IMetadataApiExtensions
    {
        /// <summary>
        /// Gets the download <see cref="Uri"/> for the specified file as an asynchronous operation.
        /// </summary>
        /// <param name="value">The <see cref="IMetadataApi"/> to get the download URI.</param>
        /// <param name="fileHash">The hash of the file to download.</param>
        /// <returns>
        /// A <see cref="Task{T}"/> containing the <see cref="Uri"/> to download the file specified by
        /// <paramref name="fileHash"/> as an asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is <see langword="null"/>.
        /// </exception>
        public static async Task<Uri> GetDownloadUriAsync(this IMetadataApi value, string fileHash)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return await value.GetDownloadUriAsync(fileHash, null, null);
        }
    }
}
