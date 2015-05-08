//-----------------------------------------------------------------------
// <copyright file="IMetadataApi.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace Experian.Qas.Updates.Metadata.WebApi.V1
{
    /// <summary>
    /// Defines the interface to the QAS Electronic Updates Metadata API.
    /// </summary>
    public interface IMetadataApi
    {
        /// <summary>
        /// Gets the URI of the QAS Electronic Updates Metadata API.
        /// </summary>
        Uri ServiceUri { get; }

        /// <summary>
        /// Gets the user name to use to authenticate with the service.
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Returns the available updates packages as an asynchronous operation.
        /// </summary>
        /// <returns>
        /// A <see cref="Task{T}"/> representing the available updates packages as an asynchronous operation.
        /// </returns>
        Task<AvailablePackagesReply> GetAvailablePackagesAsync();

        /// <summary>
        /// Gets the download <see cref="Uri"/> for the specified file as an asynchronous operation.
        /// </summary>
        /// <param name="fileName">The name of the file to download.</param>
        /// <param name="fileHash">The hash of the file to download.</param>
        /// <param name="startAtByte">The optional byte to start downloading the file from.</param>
        /// <param name="endAtByte">The optional byte to end downloading the file from.</param>
        /// <returns>
        /// A <see cref="Task{T}"/> containing the <see cref="Uri"/> to download the file specified by
        /// <paramref name="fileName"/> and <paramref name="fileHash"/> from as an asynchronous operation.
        /// </returns>
        Task<Uri> GetDownloadUriAsync(string fileName, string fileHash, long? startAtByte, long? endAtByte);

        /// <summary>
        /// Sets the credentials to use to authenticate with the service.
        /// </summary>
        /// <param name="userName">The web service user name.</param>
        /// <param name="password">The web service password.</param>
        void SetCredentials(string userName, string password);
    }
}