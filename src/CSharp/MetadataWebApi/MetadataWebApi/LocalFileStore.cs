//-----------------------------------------------------------------------
// <copyright file="LocalFileStore.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace Experian.Qas.Updates.Metadata.WebApi.V1
{
    /// <summary>
    /// A class representing a file store that stores the available data files in a
    /// serialized data file in the application directory. This class cannot be inherited.
    /// </summary>
    public sealed class LocalFileStore : IFileStore
    {
        /// <summary>
        /// The name of the file containing the serialized data.
        /// </summary>
        private readonly string _dataFileName = "FileStore.eu";

        /// <summary>
        /// A dictionary containing the map of file hashes to paths.
        /// </summary>
        private readonly IDictionary<string, string> _fileStore = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Whether the instance has been disposed.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalFileStore" /> class.
        /// </summary>
        public LocalFileStore()
            : this("FileStore.eu")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalFileStore" /> class.
        /// </summary>
        /// <param name="dataFileName">The name of the data file storing information about the downloaded files.</param>
        internal LocalFileStore(string dataFileName)
        {
            _dataFileName = dataFileName;

            if (File.Exists(_dataFileName))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(ConcurrentDictionary<string, string>));

                // Deserialize the available files from the local file store data file
                using (Stream stream = File.OpenRead(_dataFileName))
                {
                    IDictionary<string, string> data = serializer.ReadObject(stream) as IDictionary<string, string>;

                    if (data != null)
                    {
                        foreach (var pair in data)
                        {
                            _fileStore[pair.Key] = pair.Value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="LocalFileStore" /> class.
        /// </summary>
        ~LocalFileStore()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Returns whether a file with the specified hash exists in the file store.
        /// </summary>
        /// <param name="hash">The hash of the data file to check for existence.</param>
        /// <returns>
        /// <see langword="true" /> if a file with the hash specified by <paramref name="hash" />
        /// exists in the file store; otherwise <see langword="false" />.
        /// </returns>
        public bool ContainsFile(string hash)
        {
            return _fileStore.ContainsKey(hash);
        }

        /// <summary>
        /// Registers the specified file with the file store.
        /// </summary>
        /// <param name="hash">The hash of the file to register.</param>
        /// <param name="path">The path of the file to register.</param>
        public void RegisterFile(string hash, string path)
        {
            _fileStore[hash] = Path.GetFullPath(path);
        }

        /// <summary>
        /// Attempts to retrieve the path of the file with the specified
        /// hash from the file store.
        /// </summary>
        /// <param name="hash">The hash of the file to attempt to get the path of.</param>
        /// <param name="path">When the method returns, contains the path of the file with the hash
        /// specified by <paramref name="hash" /> if found; otherwise <see langword="null" />.</param>
        /// <returns>
        /// <see langword="true" /> if a file with the hash specified by <paramref name="hash" />
        /// was found in the file store; otherwise <see langword="false" />.
        /// </returns>
        public bool TryGetFilePath(string hash, out string path)
        {
            return _fileStore.TryGetValue(hash, out path);
        }

        /// <summary>
        /// Releases unmanaged and, optionally, managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true" /> to release both managed and unmanaged resources;
        /// <see langword="false" /> to release only unmanaged resources.
        /// </param>
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose of any managed resources
                }

                // Serialize the data currently stored in memory to disk
                DataContractSerializer serializer = new DataContractSerializer(typeof(ConcurrentDictionary<string, string>));

                using (Stream stream = File.Create(_dataFileName))
                {
                    serializer.WriteObject(stream, _fileStore);
                }

                _disposed = true;
            }
        }
    }
}
