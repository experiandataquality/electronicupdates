//-----------------------------------------------------------------------
// <copyright file="MetadataApiFactoryTests.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Configuration;
using Moq;
using Xunit;

namespace Experian.Qas.Updates.Metadata.WebApi.V1
{
    public static class MetadataApiFactoryTests
    {
        [Fact]
        public static void MetadataApiFactory_CreateMetadataApi_Creates_Instance()
        {
            // Arrange
            MetadataApiFactory target = new MetadataApiFactory();

            // Act
            IMetadataApi result = target.CreateMetadataApi();

            // Assert
            Assert.NotNull(result);
            Assert.IsType(typeof(MetadataApi), result);
            Assert.Equal(new Uri("https://ws.updates.qas.com/metadata/V1/"), result.ServiceUri);
            Assert.Equal("MyUserName", result.UserName);
        }

        [Fact]
        public static void MetadataApiFactory_CreateMetadataApi_Creates_Instance_If_Service_Uri_Configured()
        {
            // Arrange
            Mock<MetadataApiFactory> mock = new Mock<MetadataApiFactory>();

            mock.CallBase = true;
            mock.Setup((p) => p.GetConfigSetting("ServiceUri")).Returns("https://localhost/metadata/V1/");

            MetadataApiFactory target = mock.Object;

            // Act
            IMetadataApi result = target.CreateMetadataApi();

            // Assert
            Assert.NotNull(result);
            Assert.IsType(typeof(MetadataApi), result);
            Assert.Equal(new Uri("https://localhost/metadata/V1/"), result.ServiceUri);
            Assert.Equal("MyUserName", result.UserName);
        }

        [Fact]
        public static void MetadataApiFactory_CreateMetadataApi_Creates_Instance_If_Invalid_Service_Uri_Configured()
        {
            // Arrange
            Mock<MetadataApiFactory> mock = new Mock<MetadataApiFactory>();

            mock.CallBase = true;
            mock.Setup((p) => p.GetConfigSetting("ServiceUri")).Returns("NotAUri");

            MetadataApiFactory target = mock.Object;

            // Act
            IMetadataApi result = target.CreateMetadataApi();

            // Assert
            Assert.NotNull(result);
            Assert.IsType(typeof(MetadataApi), result);
            Assert.Equal(new Uri("https://ws.updates.qas.com/metadata/V1/"), result.ServiceUri);
            Assert.Equal("MyUserName", result.UserName);
        }

        [Fact]
        public static void MetadataApiFactory_CreateMetadataApi_Throws_If_No_User_Name_Configured()
        {
            // Arrange
            Mock<MetadataApiFactory> mock = new Mock<MetadataApiFactory>();

            mock.CallBase = true;
            mock.Setup((p) => p.GetConfigSetting("UserName")).Returns(string.Empty);
            mock.Setup((p) => p.GetConfigSetting("Password")).Returns("MyPassword");

            MetadataApiFactory target = mock.Object;

            // Act and Assert
            Assert.Throws<ConfigurationErrorsException>(() => target.CreateMetadataApi());
        }

        [Fact]
        public static void MetadataApiFactory_CreateMetadataApi_Throws_If_No_Password_Configured()
        {
            // Arrange
            Mock<MetadataApiFactory> mock = new Mock<MetadataApiFactory>();

            mock.CallBase = true;
            mock.Setup((p) => p.GetConfigSetting("UserName")).Returns("MyUserName");
            mock.Setup((p) => p.GetConfigSetting("Password")).Returns(string.Empty);

            MetadataApiFactory target = mock.Object;

            // Act and Assert
            Assert.Throws<ConfigurationErrorsException>(() => target.CreateMetadataApi());
        }

        [Fact]
        public static void MetadataApiFactory_GetAppSetting_Reads_Settings_Correctly()
        {
            // Arrange
            Environment.SetEnvironmentVariable("QAS:ElectronicUpdates:foo", "bar");

            // Act and Assert
            Assert.Equal("bar", MetadataApiFactory.GetAppSetting("foo"));
            Assert.Equal("MyPassword", MetadataApiFactory.GetAppSetting("Password"));
            Assert.Equal("MyUserName", MetadataApiFactory.GetAppSetting("UserName"));
            Assert.Equal(string.Empty, MetadataApiFactory.GetAppSetting(null));
            Assert.Equal(string.Empty, MetadataApiFactory.GetAppSetting(string.Empty));
            Assert.Equal(string.Empty, MetadataApiFactory.GetAppSetting("Foobar"));
        }
    }
}