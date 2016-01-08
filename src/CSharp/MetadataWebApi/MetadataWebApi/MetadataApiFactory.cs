//-----------------------------------------------------------------------
// <copyright file="MetadataApiFactory.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Configuration;
using System.Globalization;

namespace Experian.Qas.Updates.Metadata.WebApi.V1
{
    /// <summary>
    /// A class representing the default implementation of <see cref="IMetadataApiFactory"/>.
    /// </summary>
    public class MetadataApiFactory : IMetadataApiFactory
    {
        /// <summary>
        /// Creates a new instance of <see cref="IMetadataApi"/>.
        /// </summary>
        /// <returns>
        /// The created instance of <see cref="IMetadataApi"/>.
        /// </returns>
        /// <exception cref="ConfigurationErrorsException">
        /// The application is not configured correctly to create an instance of <see cref="IMetadataApi"/>.
        /// </exception>
        public virtual IMetadataApi CreateMetadataApi()
        {
            // Get the credentials to use to connect to the QAS Electronic Updates Metadata REST API
            string userName = GetConfigSetting("UserName");
            string password = GetConfigSetting("Password");

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                throw new ConfigurationErrorsException("No QAS Electronic Updates service credentials are configured.");
            }

            // Has the REST API endpoint URI been overridden?
            string serviceUrl = GetConfigSetting("ServiceUri");

            Uri serviceUri;

            if (!Uri.TryCreate(serviceUrl, UriKind.Absolute, out serviceUri))
            {
                serviceUri = new Uri("https://ws.updates.qas.com/metadata/V1/");
            }

            // Create the service implementation
            IMetadataApi service = new MetadataApi(serviceUri);

            // Set the credentials to use to authenticate with the service
            service.SetCredentials(userName, password);

            return service;
        }

        /// <summary>
        /// Gets the application configuration setting with the specified name.
        /// </summary>
        /// <param name="name">The name of the application configuration setting to obtain.</param>
        /// <returns>
        /// The value of the specified application configuration setting, if found; otherwise <see cref="string.Empty"/>.
        /// </returns>
        internal static string GetAppSetting(string name)
        {
            // Build the full name of the setting
            string settingName = string.Format(CultureInfo.InvariantCulture, "QAS:ElectronicUpdates:{0}", name);

            // Is the setting configured in the application configuration file?
            string value = ConfigurationManager.AppSettings[settingName];

            // If not, try the environment variables
            if (string.IsNullOrEmpty(value))
            {
                settingName = string.Format(CultureInfo.InvariantCulture, "QAS_ElectronicUpdates_{0}", name);
                value = Environment.GetEnvironmentVariable(settingName);
            }

            return value ?? string.Empty;
        }

        /// <summary>
        /// Gets the configuration setting with the specified name.
        /// </summary>
        /// <param name="name">The name of the configuration setting to obtain.</param>
        /// <returns>
        /// The value of the specified configuration setting, if found; otherwise <see cref="string.Empty"/>.
        /// </returns>
        protected internal virtual string GetConfigSetting(string name)
        {
            return GetAppSetting(name);
        }
    }
}
