//-----------------------------------------------------------------------
// <copyright file="MetadataApiFactoryTests.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Configuration;
using Moq;
using Xunit;

namespace Experian.Qas.Updates.Metadata.WebApi.V2
{
    public static class MetadataApiFactoryTests
    {
        [Fact]
        public static void MetadataApiFactory_CreateMetadataApi_Creates_Instance()
        {
            // Arrange
            Mock<MetadataApiFactory> mock = new Mock<MetadataApiFactory>();

            mock.CallBase = true;
            mock.Setup((p) => p.GetConfigSetting("Token")).Returns("AuthToken");

            Uri expectedUri;

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("QAS_ElectronicUpdates_ServiceUri")))
            {
                expectedUri = new Uri(Environment.GetEnvironmentVariable("QAS_ElectronicUpdates_ServiceUri"));
            }
            else
            {
                expectedUri = new Uri("https://ws.updates.qas.com/metadata/V2/");
            }

            MetadataApiFactory target = mock.Object;

            // Act
            IMetadataApi result = target.CreateMetadataApi();

            // Assert
            Assert.NotNull(result);
            Assert.IsType(typeof(MetadataApi), result);
            Assert.Equal(expectedUri, result.ServiceUri);
            Assert.Equal("AuthToken", result.Token);
        }

        [Fact]
        public static void MetadataApiFactory_CreateMetadataApi_Creates_Instance_If_Service_Uri_Configured()
        {
            // Arrange
            Mock<MetadataApiFactory> mock = new Mock<MetadataApiFactory>();

            mock.CallBase = true;
            mock.Setup((p) => p.GetConfigSetting("ServiceUri")).Returns("https://localhost/metadata/V2/");
            mock.Setup((p) => p.GetConfigSetting("Token")).Returns("AuthToken");

            MetadataApiFactory target = mock.Object;

            // Act
            IMetadataApi result = target.CreateMetadataApi();

            // Assert
            Assert.NotNull(result);
            Assert.IsType(typeof(MetadataApi), result);
            Assert.Equal(new Uri("https://localhost/metadata/V2/"), result.ServiceUri);
            Assert.Equal("AuthToken", result.Token);
        }

        [Fact]
        public static void MetadataApiFactory_CreateMetadataApi_Creates_Instance_If_Invalid_Service_Uri_Configured()
        {
            // Arrange
            Mock<MetadataApiFactory> mock = new Mock<MetadataApiFactory>();

            mock.CallBase = true;
            mock.Setup((p) => p.GetConfigSetting("ServiceUri")).Returns("NotAUri");
            mock.Setup((p) => p.GetConfigSetting("Token")).Returns("AuthToken");

            MetadataApiFactory target = mock.Object;

            // Act
            IMetadataApi result = target.CreateMetadataApi();

            // Assert
            Assert.NotNull(result);
            Assert.IsType(typeof(MetadataApi), result);
            Assert.Equal(new Uri("https://ws.updates.qas.com/metadata/V2/"), result.ServiceUri);
            Assert.Equal("AuthToken", result.Token);
        }

        [Fact]
        public static void MetadataApiFactory_CreateMetadataApi_Throws_If_No_Token_Configured()
        {
            // Arrange
            Mock<MetadataApiFactory> mock = new Mock<MetadataApiFactory>();

            mock.CallBase = true;
            mock.Setup((p) => p.GetConfigSetting("Token")).Returns(string.Empty);

            MetadataApiFactory target = mock.Object;

            // Act and Assert
            Assert.Throws<ConfigurationErrorsException>(() => target.CreateMetadataApi());
        }

        [Fact]
        public static void MetadataApiFactory_GetAppSetting_Reads_Settings_Correctly_From_Environment_Variable()
        {
            // Arrange
            Environment.SetEnvironmentVariable("QAS_ElectronicUpdates_foo", "bar");

            // Act and Assert
            Assert.Equal("bar", MetadataApiFactory.GetAppSetting("foo"));
        }

        [Theory]
        [InlineData(null, "")]
        [InlineData("", "")]
        [InlineData("Foobar", "")]
        [InlineData("MySetting", "MyValue")]
        public static void MetadataApiFactory_GetAppSetting_Reads_Setting_Value_Correctly(string name, string value)
        {
            // Act and Assert
            Assert.Equal(value, MetadataApiFactory.GetAppSetting(name));
        }
    }
}
