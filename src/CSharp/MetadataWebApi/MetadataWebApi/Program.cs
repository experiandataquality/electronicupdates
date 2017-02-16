//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Experian.Qas.Updates.Metadata.WebApi.V2
{
    /// <summary>
    /// A class representing an example implementation of the QAS Electronic Updates Metadata API.  This class cannot be inherited.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry-point to the application.
        /// </summary>
        internal static void Main()
        {
            try
            {
                MainAsync().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unhandled exception: {0}{1}{1}{2}{1}", ex.Message, Environment.NewLine, ex);
            }

            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Downloads the available data files from the QAS Electronic Updates Metadata REST API as an asynchronous operation.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation to download any data files.
        /// </returns>
        internal static async Task MainAsync()
        {
            PrintBanner();

            // Get the configuration settings for downloading files
            string downloadRootPath = MetadataApiFactory.GetAppSetting("DownloadRootPath");
            string verifyDownloadsString = MetadataApiFactory.GetAppSetting("ValidateDownloads");

            bool verifyDownloads;

            if (!bool.TryParse(verifyDownloadsString, out verifyDownloads))
            {
                verifyDownloads = true;
            }

            if (string.IsNullOrEmpty(downloadRootPath))
            {
                downloadRootPath = "QASData";
            }

            downloadRootPath = Path.GetFullPath(downloadRootPath);

            // Create the service implementation
            IMetadataApiFactory factory = new MetadataApiFactory();
            IMetadataApi service = factory.CreateMetadataApi();

            Console.WriteLine("QAS Electronic Updates Metadata REST API: {0}", service.ServiceUri);
            Console.WriteLine();

            // Query the packages available to the account
            List<PackageGroup> response = await service.GetAvailablePackagesAsync();

            Console.WriteLine("Available Package Groups:");
            Console.WriteLine();

            // Enumerate the package groups and list their packages and files
            if (response != null && response.Count > 0)
            {
                using (CancellationTokenSource tokenSource = new CancellationTokenSource())
                {
                    // Cancel the tasks if Ctrl+C is entered to the console
                    Console.CancelKeyPress += (sender, e) =>
                    {
                        if (!tokenSource.IsCancellationRequested)
                        {
                            tokenSource.Cancel();
                        }

                        e.Cancel = true;
                    };

                    try
                    {
                        Stopwatch stopwatch = Stopwatch.StartNew();

                        // Create a file store in which to cache information about which files
                        // have already been downloaded from the Metadata API service.
                        using (IFileStore fileStore = new LocalFileStore())
                        {
                            foreach (PackageGroup group in response)
                            {
                                Console.WriteLine("Group Name: {0} ({1})", group.PackageGroupCode, group.Vintage);
                                Console.WriteLine();
                                Console.WriteLine("Packages:");
                                Console.WriteLine();

                                foreach (Package package in group.Packages)
                                {
                                    Console.WriteLine("Package Name: {0}", package.PackageCode);
                                    Console.WriteLine();
                                    Console.WriteLine("Files:");
                                    Console.WriteLine();

                                    foreach (DataFile file in package.Files)
                                    {
                                        if (fileStore.ContainsFile(file.MD5Hash))
                                        {
                                            // We already have this file, download not required
                                            Console.WriteLine("File with hash '{0}' already downloaded.", file.MD5Hash);
                                        }
                                        else
                                        {
                                            // Download the file
                                            await DownloadFileAsync(
                                                service,
                                                fileStore,
                                                group,
                                                file,
                                                downloadRootPath,
                                                verifyDownloads,
                                                tokenSource.Token);
                                        }
                                    }

                                    Console.WriteLine();
                                }

                                Console.WriteLine();
                            }
                        }

                        stopwatch.Stop();
                        Console.WriteLine("Downloaded data in {0:hh\\:mm\\:ss}.", stopwatch.Elapsed);
                    }
                    catch (OperationCanceledException ex)
                    {
                        // Only an error if not cancelled by the user
                        if (ex.CancellationToken != tokenSource.Token)
                        {
                            throw;
                        }

                        Console.WriteLine("File download cancelled by user.");
                    }

                    Console.WriteLine();
                }
            }
        }

        /// <summary>
        /// Downloads the specified file from the specified package group as an asynchronous operation.
        /// </summary>
        /// <param name="service">The Metadata API to use to get the download URI.</param>
        /// <param name="fileStore">The file store to register the download with if successfully downloaded.</param>
        /// <param name="group">The package group to download the file from.</param>
        /// <param name="file">The data file to download.</param>
        /// <param name="downloadRootPath">The root path to download data to.</param>
        /// <param name="verifyDownloads">Whether to verify file downloads.</param>
        /// <param name="cancellationToken">The cancellation token to use to cancel any downloads.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        private static async Task DownloadFileAsync(
            IMetadataApi service,
            IFileStore fileStore,
            PackageGroup group,
            DataFile file,
            string downloadRootPath,
            bool verifyDownloads,
            CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            Console.WriteLine("File name: {0}", file.FileName);
            Console.WriteLine("File hash: {0}", file.MD5Hash);
            Console.WriteLine("File size: {0:N0}", file.Size);

            // Query the URIs to download the file from
            Uri uri = await service.GetDownloadUriAsync(file.FileName, file.MD5Hash);

            if (uri == null)
            {
                Console.WriteLine("File '{0}' is not available for download at this time.", file.FileName);
                Console.WriteLine();
                return;
            }

            Console.WriteLine("File URI: {0}", uri);
            Console.WriteLine();

            // Create the path to the directory to download the file to
            string directoryPath = Path.Combine(
                downloadRootPath,
                group.PackageGroupCode,
                group.Vintage);

            string filePath = Path.GetFullPath(Path.Combine(directoryPath, file.FileName));

            // Create the directory if it doesn't already exist
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Download the file
            Console.WriteLine("Downloading '{0}' ({1}) to '{2}'...", file.FileName, file.MD5Hash, filePath);

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(uri, cancellationToken))
                {
                    response.EnsureSuccessStatusCode();

                    using (Stream stream = await response.Content.ReadAsStreamAsync())
                    {
                        using (Stream fileStream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                        {
                            // Ensure any existing file is overwritten
                            fileStream.SetLength(0L);

                            await stream.CopyToAsync(fileStream, 4096, cancellationToken);
                        }
                    }
                }
            }

            // Validate the download is correct, if configured.
            // Don't register the file in the file store as the file download became corrupted somehow.
            if (!verifyDownloads || VerifyDownload(filePath, file.MD5Hash))
            {
                // Register the file with the file store so further invocations
                // of the application don't unnecessarily download the file again
                fileStore.RegisterFile(file.MD5Hash, filePath);
            }
        }

        /// <summary>
        /// Validates that the specified file was downloaded correctly.
        /// </summary>
        /// <param name="filePath">The path of the downloaded file.</param>
        /// <param name="expectedHash">The expected hash of <paramref name="filePath"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the hash of the specified file is correct; otherwise <see langword="false"/>.
        /// </returns>
        private static bool VerifyDownload(string filePath, string expectedHash)
        {
            Console.WriteLine("Validating hash of '{0}'...", filePath);

            bool isHashCorrect;

            filePath = Path.GetFullPath(filePath);

            using (HashAlgorithm algorithm = HashAlgorithm.Create("MD5"))
            {
                using (Stream stream = File.OpenRead(filePath))
                {
                    byte[] hash = algorithm.ComputeHash(stream);

                    StringBuilder builder = new StringBuilder();

                    foreach (byte b in hash)
                    {
                        builder.AppendFormat(CultureInfo.InvariantCulture, "{0:x2}", b);
                    }

                    isHashCorrect = string.Equals(builder.ToString(), expectedHash, StringComparison.Ordinal);

                    if (!isHashCorrect)
                    {
                        Console.WriteLine(
                            "The MD5 hash of '{0}' ({1}) does not match the expected hash of '{2}'.  A HTTP transmission error is likely to have occurred.",
                            filePath,
                            builder,
                            expectedHash);
                    }
                }
            }

            Console.WriteLine();
            return isHashCorrect;
        }

        /// <summary>
        /// Prints the application banner to the console.
        /// </summary>
        private static void PrintBanner()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            AssemblyName currentAssembly = assembly.GetName();

            var copyright = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>();
            var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

            string welcomeMessage = string.Format(
                CultureInfo.InvariantCulture,
                "{0}{1} (v{2}) - {3}{0}",
                Environment.NewLine,
                currentAssembly.Name,
                version.InformationalVersion,
                copyright.Copyright);

            Console.WriteLine(welcomeMessage);
        }
    }
}
