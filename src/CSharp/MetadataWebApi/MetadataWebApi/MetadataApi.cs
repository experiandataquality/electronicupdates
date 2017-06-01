//-----------------------------------------------------------------------
// <copyright file="MetadataApi.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Experian.Qas.Updates.Metadata.WebApi.V2
{
    /// <summary>
    /// A class representing the default of implementation of <see cref="IMetadataApi"/> to access the Experian Data Quality Electronic Updates Metadata API.
    /// </summary>
    [DebuggerDisplay("{ServiceUri}")]
    public class MetadataApi : IMetadataApi
    {
        /// <summary>
        /// The Electronic Updates Metadata API URI. This field is read-only.
        /// </summary>
        private readonly Uri _serviceUri;

        /// <summary>
        /// The authentication token used to communicate with the Metadata API.
        /// </summary>
        private string _token;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataApi"/> class.
        /// </summary>
        /// <param name="serviceUri">The Electronic Updates Metadata API service URI.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serviceUri"/> is <see langword="null"/>.
        /// </exception>
        public MetadataApi(Uri serviceUri)
        {
            if (serviceUri == null)
            {
                throw new ArgumentNullException("serviceUri");
            }

            _serviceUri = serviceUri;
        }

        /// <summary>
        /// Gets the URI of the Electronic Updates Metadata API.
        /// </summary>
        public Uri ServiceUri
        {
            get { return _serviceUri; }
        }

        /// <summary>
        /// Gets the authentication token used to communicate with the Metadata API.
        /// </summary>
        public string Token
        {
            get { return _token; }
        }

        /// <summary>
        /// Gets the HTTP request content type.
        /// </summary>
        protected virtual string ContentType
        {
            get { return "application/json"; }
        }

        /// <summary>
        /// Returns the available updates packages as an asynchronous operation.
        /// </summary>
        /// <returns>
        /// A <see cref="Task{T}"/> representing the available updates packages as an asynchronous operation.
        /// </returns>
        /// <exception cref="MetadataApiException">
        /// The available packages could not be retrieved.
        /// </exception>
        public virtual async Task<List<PackageGroup>> GetAvailablePackagesAsync()
        {
            try
            {
                using (HttpClient client = CreateHttpClient())
                {
                    var tokenHeader = string.Format(CultureInfo.InvariantCulture, "x-api-key {0}", _token);
                    client.DefaultRequestHeaders.Add("Authorization", tokenHeader);

                    using (HttpResponseMessage response = await client.GetAsync("packages"))
                    {
                        response.EnsureSuccessStatusCode();
                        var raw = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<List<PackageGroup>>(raw);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new MetadataApiException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Gets the download <see cref="Uri"/> for the specified file as an asynchronous operation.
        /// </summary>
        /// <param name="fileName">The name of the file to download.</param>
        /// <param name="fileHash">The hash of the file to download.</param>
        /// <param name="startAtByte">The optional byte to start downloading the file from.</param>
        /// <param name="endAtByte">The optional byte to end downloading the file from.</param>
        /// <returns>
        /// A <see cref="Task{T}"/> containing the <see cref="Uri"/> to download the file specified by
        /// <paramref name="fileName"/> and <paramref name="fileHash"/> from as an asynchronous operation.
        /// </returns>
        /// <exception cref="MetadataApiException">
        /// The download URI could not be obtained.
        /// </exception>
        public virtual async Task<Uri> GetDownloadUriAsync(string fileName, string fileHash, long? startAtByte, long? endAtByte)
        {
            GetDownloadUriRequest request = new GetDownloadUriRequest()
            {
                FileMD5Hash = fileHash,
                FileName = fileName,
                StartAtByte = startAtByte,
                EndAtByte = endAtByte,
            };

            try
            {
                FileDownloadReply downloadResponse = await Post<GetDownloadUriRequest, FileDownloadReply>("filelink", request, _token);

                if (downloadResponse == null || downloadResponse.DownloadUri == null)
                {
                    return null;
                }

                return new Uri(downloadResponse.DownloadUri);
            }
            catch (Exception ex)
            {
                throw new MetadataApiException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Sets the token used to authenticate with the service.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        public virtual void SetToken(string token)
        {
            _token = token;
        }

        /// <summary>
        /// Creates a new instance of <see cref="HttpClient"/> that can be used to consume
        /// the Electronic Updates Metadata Web API.
        /// </summary>
        /// <returns>
        /// The created instance of <see cref="HttpClient"/>.
        /// </returns>
        protected virtual HttpClient CreateHttpClient()
        {
            var assembly = Assembly.GetEntryAssembly();
            var assemblyName = assembly.GetName();

            var contentTypeHeader = new MediaTypeWithQualityHeaderValue(this.ContentType);
            var userAgentHeader = new ProductInfoHeaderValue(assemblyName.Name, assemblyName.Version.ToString());

            HttpClient client = new HttpClient();

            try
            {
                client.BaseAddress = _serviceUri;

                // Add any other default and/or custom HTTP request headers here
                client.DefaultRequestHeaders.Accept.Add(contentTypeHeader);
                client.DefaultRequestHeaders.UserAgent.Add(userAgentHeader);

                return client;
            }
            catch (Exception)
            {
                client.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Posts the specified value to the specified relative URI and reads the response as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResult">The type of the response.</typeparam>
        /// <param name="requestUri">The relative URI the request is sent to.</param>
        /// <param name="value">The value to write into the entity body of the request.</param>
        /// <param name="token">The authentication token value.</param>
        /// <returns>
        /// A <see cref="Task{T}"/> that will yield an instance of <typeparamref name="TResult"/> read from the response as an asynchronous operation.
        /// </returns>
        protected virtual async Task<TResult> Post<TRequest, TResult>(string requestUri, TRequest value, string token)
        {
            using (HttpClient client = CreateHttpClient())
            {
                var tokenHeader = string.Format(CultureInfo.InvariantCulture, "x-api-key {0}", token);
                client.DefaultRequestHeaders.Add("Authorization", tokenHeader);

                var json = new StringContent(JsonConvert.SerializeObject(value).ToString(), Encoding.UTF8, "application/json");
                using (HttpResponseMessage response = await client.PostAsync(requestUri, json))
                {
                    response.EnsureSuccessStatusCode();
                    var raw = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TResult>(raw);
                }
            }
        }
    }
}
