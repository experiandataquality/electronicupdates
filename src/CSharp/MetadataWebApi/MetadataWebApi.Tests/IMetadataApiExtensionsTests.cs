//-----------------------------------------------------------------------
// <copyright file="IMetadataApiExtensionsTests.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Xunit;

namespace Experian.Qas.Updates.Metadata.WebApi.V2
{
    public static class IMetadataApiExtensionsTests
    {
        [Fact]
        public static async Task IMetadataApiExtensions_GetDownloadUriAsync_Throws_If_Value_Is_Null()
        {
            // Arrange
            IMetadataApi value = null;

            string fileName = "MyFileName.dts";
            string fileHash = "58b653e3762e8048995e00024a512c53";

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>("value", () => value.GetDownloadUriAsync(fileName, fileHash));
        }
    }
}
