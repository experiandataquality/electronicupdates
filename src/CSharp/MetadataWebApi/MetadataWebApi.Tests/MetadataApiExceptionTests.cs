//-----------------------------------------------------------------------
// <copyright file="MetadataApiExceptionTests.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Xunit;

namespace Experian.Qas.Updates.Metadata.WebApi.V1
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
            Assert.Equal("An error was returned by the QAS Electronic Updates Metadata REST API.", target.Message);
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

        [Fact]
        public static void MetadataApiException_Constructor_For_Serialization_Can_Be_Serialized()
        {
            // Arrange
            InvalidOperationException innerException = new InvalidOperationException();
            string message = Guid.NewGuid().ToString();

            // Act
            MetadataApiException target = new MetadataApiException(message, innerException);

            BinaryFormatter formatter = new BinaryFormatter();

            MetadataApiException deserialized;

            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, target);
                stream.Seek(0L, SeekOrigin.Begin);
                deserialized = formatter.Deserialize(stream) as MetadataApiException;
            }

            // Assert
            Assert.NotNull(deserialized);
            Assert.NotNull(deserialized.InnerException);
            Assert.IsType(innerException.GetType(), deserialized.InnerException);
            Assert.Equal(deserialized.Message, target.Message);
        }
    }
}
