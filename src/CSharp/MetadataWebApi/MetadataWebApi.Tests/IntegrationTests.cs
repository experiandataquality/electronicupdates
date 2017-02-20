//-----------------------------------------------------------------------
// <copyright file="IntegrationTests.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Experian.Qas.Updates.Metadata.WebApi.V2
{
    public class IntegrationTests
    {
        private readonly ITestOutputHelper _output;

        public IntegrationTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [RequiresServiceCredentialsFact]
        public static async Task MetadataApi_GetAvailablePackagesAsync_Returns_Available_Packages()
        {
            // Arrange
            MetadataApi target = CreateAuthorizedService();

            // Act
            List<PackageGroup> result = await target.GetAvailablePackagesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(0, result.Count);
            Assert.DoesNotContain(null, result);

            Assert.All(
                result,
                (group) =>
                {
                    Assert.False(string.IsNullOrEmpty(group.PackageGroupCode));
                    Assert.False(string.IsNullOrEmpty(group.Vintage));

                    Assert.NotNull(group.Packages);
                    Assert.NotEqual(0, group.Packages.Count);
                    Assert.DoesNotContain(null, group.Packages);

                    Assert.All(
                        group.Packages,
                        (package) =>
                        {
                            Assert.False(string.IsNullOrEmpty(package.PackageCode));

                            Assert.NotNull(package.Files);
                            Assert.NotEqual(0, package.Files.Count);
                            Assert.DoesNotContain(null, package.Files);

                            Assert.All(
                                package.Files,
                                (file) =>
                                {
                                    Assert.False(string.IsNullOrEmpty(file.FileName));
                                    Assert.False(string.IsNullOrEmpty(file.MD5Hash));
                                    Assert.True(file.Size > 0L);
                                });
                        });
                });
        }

        [RequiresServiceCredentialsFact]
        public static async Task MetadataApi_GetDownloadUriAsync_Returns_File_Download_Uri_Which_Downloads_Correct_File()
        {
            // Arrange
            MetadataApi target = CreateAuthorizedService();

            List<PackageGroup> packages = await target.GetAvailablePackagesAsync();

            DataFile dataFile = packages
                .SelectMany((p) => p.Packages)
                .First()
                .Files
                .First();

            // Act
            Uri result = await target.GetDownloadUriAsync(dataFile.MD5Hash);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsAbsoluteUri);

            string tempPath = Path.Combine(Path.GetTempPath(), dataFile.FileName);

            try
            {
                using (WebClient client = new WebClient())
                {
                    await client.DownloadFileTaskAsync(result, tempPath);
                }

                Assert.True(File.Exists(tempPath));

                byte[] hash;

                using (Stream stream = File.OpenRead(tempPath))
                {
                    Assert.Equal(dataFile.Size, stream.Length);

                    using (HashAlgorithm algorithm = HashAlgorithm.Create("MD5"))
                    {
                        hash = algorithm.ComputeHash(stream);
                    }
                }

                string hashString = string.Concat(hash.Select((p) => p.ToString("x2", CultureInfo.InvariantCulture)));

                Assert.Equal(dataFile.MD5Hash, hashString);
            }
            finally
            {
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }
            }
        }

        [RequiresServiceCredentialsFact]
        public async Task Program_MainAsync_Downloads_Data_Files()
        {
            // Arrange
            const string FileStoreName = "FileStore.eu";

            if (File.Exists(FileStoreName))
            {
                File.Delete(FileStoreName);
            }

            using (TextWriter writer = new StringWriter(CultureInfo.InvariantCulture))
            {
                Console.SetOut(writer);

                try
                {
                    // Act
                    await Program.MainAsync();

                    // Assert
                    Assert.True(Directory.Exists("QASData"));

                    string[] filePaths = Directory.GetFiles("QASData", "*", SearchOption.AllDirectories);

                    // Verify at least one non-empty file was downloaded
                    Assert.NotEmpty(filePaths);
                    Assert.All(filePaths, (p) => Assert.NotEqual(0L, new FileInfo(p).Length));
                }
                finally
                {
                    _output.WriteLine(writer.ToString());
                }
            }
        }

        private static MetadataApi CreateAuthorizedService()
        {
            MetadataApiFactory factory = new MetadataApiFactory();
            return factory.CreateMetadataApi() as MetadataApi;
        }
    }
}
