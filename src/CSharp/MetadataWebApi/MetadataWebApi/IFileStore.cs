//-----------------------------------------------------------------------
// <copyright file="IFileStore.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace Experian.Qas.Updates.Metadata.WebApi.V1
{
    /// <summary>
    /// Defines a file store of data files.
    /// </summary>
    public interface IFileStore : IDisposable
    {
        /// <summary>
        /// Returns whether a file with the specified hash exists in the file store.
        /// </summary>
        /// <param name="hash">The hash of the data file to check for existence.</param>
        /// <returns>
        /// <see langword="true"/> if a file with the hash specified by <paramref name="hash"/>
        /// exists in the file store; otherwise <see langword="false"/>.
        /// </returns>
        bool ContainsFile(string hash);

        /// <summary>
        /// Registers the specified file with the file store.
        /// </summary>
        /// <param name="hash">The hash of the file to register.</param>
        /// <param name="path">The path of the file to register.</param>
        void RegisterFile(string hash, string path);

        /// <summary>
        /// Attempts to retrieve the path of the file with the specified
        /// hash from the file store.
        /// </summary>
        /// <param name="hash">The hash of the file to attempt to get the path of.</param>
        /// <param name="path">
        /// When the method returns, contains the path of the file with the hash
        /// specified by <paramref name="hash"/> if found; otherwise <see langword="null"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if a file with the hash specified by <paramref name="hash"/>
        /// was found in the file store; otherwise <see langword="false"/>.
        /// </returns>
        bool TryGetFilePath(string hash, out string path);
    }
}
