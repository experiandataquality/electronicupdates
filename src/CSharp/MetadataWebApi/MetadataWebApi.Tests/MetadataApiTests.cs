//-----------------------------------------------------------------------
// <copyright file="MetadataApiTests.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Experian.Qas.Updates.Metadata.WebApi.V1
{
    public static class MetadataApiTests
    {
        [Fact]
        public static void MetadataApi_Constructor_Throws_If_ServiceUri_Is_Null()
        {
            // Arrange
            Uri serviceUri = null;

            // Act and Assert
            Assert.Throws<ArgumentNullException>("serviceUri", () => new MetadataApi(serviceUri));
        }

        [Fact]
        public static void MetadataApi_GetAvailablePackagesAsync_Throws_MetadataApiException_If_Error_Occurs()
        {
            // Arrange
            Uri serviceUri = new Uri("https://ws.updates.qas.com/metadata/V1/");
            MetadataApi target = new MetadataApi(serviceUri);

            // Act and Assert - Should throw as no credentials configured
            Assert.ThrowsAsync<MetadataApiException>(() => target.GetAvailablePackagesAsync());
        }

        [Fact]
        public static void MetadataApi_GetDownloadUriAsync_Throws_MetadataApiException_If_Error_Occurs()
        {
            // Arrange
            Uri serviceUri = new Uri("https://ws.updates.qas.com/metadata/V1/");
            MetadataApi target = new MetadataApi(serviceUri);

            string fileName = "MyFile.txt";
            string fileHash = "7039d49e15fd4e164e2c07fe76fd61a2";
            long? startAtByte = null;
            long? endAtByte = null;

            // Act and Assert - Should throw as no credentials configured
            Assert.ThrowsAsync<MetadataApiException>(() => target.GetDownloadUriAsync(fileName, fileHash, startAtByte, endAtByte));
        }

        [RequiresServiceCredentialsFact]
        public static async Task MetadataApi_GetAvailablePackagesAsync_Returns_Available_Packages()
        {
            // Arrange
            MetadataApi target = CreateAuthorizedService();

            // Act
            AvailablePackagesReply result = await target.GetAvailablePackagesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.PackageGroups);
            Assert.NotEqual(0, result.PackageGroups.Count);
            Assert.DoesNotContain(null, result.PackageGroups);

            Assert.All(
                result.PackageGroups,
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
        public static async Task MetadataApi_GetDownloadUriAsync_Returns_File_Download_Uri()
        {
            // Arrange
            MetadataApi target = CreateAuthorizedService();

            AvailablePackagesReply packages = await target.GetAvailablePackagesAsync();

            DataFile dataFile = packages.PackageGroups
                .SelectMany((p) => p.Packages)
                .First()
                .Files
                .First();

            // Act
            Uri result = await target.GetDownloadUriAsync(dataFile.FileName, dataFile.MD5Hash, 0L, dataFile.Size);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsAbsoluteUri);
        }

        private static MetadataApi CreateAuthorizedService()
        {
            MetadataApiFactory factory = new MetadataApiFactory();
            return factory.CreateMetadataApi() as MetadataApi;
        }
    }
}