//-----------------------------------------------------------------------
// <copyright file="MetadataApiTests.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Xunit;

namespace Experian.Qas.Updates.Metadata.WebApi.V2
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
        public static async Task MetadataApi_GetAvailablePackagesAsync_Throws_MetadataApiException_If_Error_Occurs()
        {
            // Arrange
            Uri serviceUri = new Uri("https://ws.updates.qas.com/metadata/V2/");
            MetadataApi target = new MetadataApi(serviceUri);

            // Act and Assert - Should throw as no credentials configured
            await Assert.ThrowsAsync<MetadataApiException>(() => target.GetAvailablePackagesAsync());
        }

        [Fact]
        public static async Task MetadataApi_GetDownloadUriAsync_Throws_MetadataApiException_If_Error_Occurs()
        {
            // Arrange
            Uri serviceUri = new Uri("https://ws.updates.qas.com/metadata/V2/");
            MetadataApi target = new MetadataApi(serviceUri);

            string fileHash = "7039d49e15fd4e164e2c07fe76fd61a2";
            long? startAtByte = null;
            long? endAtByte = null;

            // Act and Assert - Should throw as no credentials configured
            await Assert.ThrowsAsync<MetadataApiException>(() => target.GetDownloadUriAsync(fileHash, startAtByte, endAtByte));
        }
    }
}
