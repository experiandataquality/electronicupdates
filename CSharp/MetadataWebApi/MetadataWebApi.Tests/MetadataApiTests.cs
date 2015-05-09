//-----------------------------------------------------------------------
// <copyright file="MetadataApiTests.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
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
    }
}