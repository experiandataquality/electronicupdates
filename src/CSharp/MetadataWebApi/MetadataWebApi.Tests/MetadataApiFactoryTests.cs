//-----------------------------------------------------------------------
// <copyright file="MetadataApiFactoryTests.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using Xunit;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Experian.Qas.Updates.Metadata.WebApi.V2
{
    public class MetadataApiFactoryTests
    {
        private IDictionary<string,string> inMemoryConfig = new Dictionary<string,string>
        {
            {"appSettings:token", "AuthToken"},
            {"appSettings:downloadRootPath", ""},
            {"appSettings:validateDownloads", ""},
            {"appSettings:serviceUri", "https://ws.updates.qas.com/metadata/V2/"}
        };

        private IDictionary<string,string> inMemoryConfigEmpty = new Dictionary<string,string>
        {
            {"appSettings:token", ""},
            {"appSettings:downloadRootPath", ""},
            {"appSettings:validateDownloads", ""},
            {"appSettings:serviceUri", ""}
        };

    [Fact]
        public void MetadataApiFactory_CreateMetadataApi_Creates_Instance()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemoryConfig)
                .Build();

            // Arrange
            MetadataApiFactory target = new MetadataApiFactory(configuration);

            Uri expectedUri;

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("EDQ_ElectronicUpdates_ServiceUri")))
            {
                expectedUri = new Uri(Environment.GetEnvironmentVariable("EDQ_ElectronicUpdates_ServiceUri"));
            }
            else
            {
                expectedUri = new Uri("https://ws.updates.qas.com/metadata/V2/");
            }

            // Act
            IMetadataApi result = target.CreateMetadataApi();

            // Assert
            Assert.NotNull(result);
            Assert.IsType(typeof(MetadataApi), result);
            Assert.Equal(expectedUri, result.ServiceUri);
            Assert.Equal("AuthToken", result.Token);
        }

        [Fact]
        public void MetadataApiFactory_CreateMetadataApi_Creates_Instance_If_Service_Uri_Configured()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemoryConfig)
                .Build();

            // Arrange
            MetadataApiFactory target = new MetadataApiFactory(configuration);

            // Act
            IMetadataApi result = target.CreateMetadataApi();

            // Assert
            Assert.NotNull(result);
            Assert.IsType(typeof(MetadataApi), result);
            Assert.Equal(new Uri("https://ws.updates.qas.com/metadata/V2/"), result.ServiceUri);
            Assert.Equal("AuthToken", result.Token);
        }

        [Fact]
        public void MetadataApiFactory_CreateMetadataApi_Creates_Instance_If_Invalid_Service_Uri_Configured()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemoryConfig)
                .Build();

            // Arrange
            MetadataApiFactory target = new MetadataApiFactory(configuration);

            // Act
            IMetadataApi result = target.CreateMetadataApi();

            // Assert
            Assert.NotNull(result);
            Assert.IsType(typeof(MetadataApi), result);
            Assert.Equal(new Uri("https://ws.updates.qas.com/metadata/V2/"), result.ServiceUri);
            Assert.Equal("AuthToken", result.Token);
        }

        [Fact]
        public void MetadataApiFactory_CreateMetadataApi_Throws_If_No_Token_Configured()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemoryConfigEmpty)
                .Build();

            var target = new MetadataApiFactory(configuration);

            // Act and Assert
            Assert.Throws<NullReferenceException>(() => target.CreateMetadataApi());
        }

        [Fact]
        public static void MetadataApiFactory_GetAppSetting_Reads_Settings_Correctly_From_Environment_Variable()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> { { "appSettings:token", "AuthToken" } })
                .Build();

            MetadataApiFactory target = new MetadataApiFactory(configuration);

            Environment.SetEnvironmentVariable("EDQ_ElectronicUpdates_foo", "bar");

            // Act and Assert
            Assert.Equal("bar", target.GetConfigSetting("foo"));
        }
    }
}
