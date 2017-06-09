//-----------------------------------------------------------------------
// <copyright file="MetadataApiExceptionTests.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using Xunit;

namespace Experian.Qas.Updates.Metadata.WebApi.V2
{
    public static class MetadataApiExceptionTests
    {
        [Fact]
        public static void MetadataApiException_Default_Constructor_Initializes_Exception()
        {
            // Act
            MetadataApiException target = new MetadataApiException();

            // Assert
            Assert.Null(target.InnerException);
            Assert.Equal("An error was returned by the Electronic Updates Metadata REST API.", target.Message);
        }

        [Fact]
        public static void MetadataApiException_Constructor_With_Message_Initializes_Exception()
        {
            // Arrange
            string message = Guid.NewGuid().ToString();

            // Act
            MetadataApiException target = new MetadataApiException(message);

            // Assert
            Assert.Null(target.InnerException);
            Assert.Equal(message, target.Message);
        }
    }
}
