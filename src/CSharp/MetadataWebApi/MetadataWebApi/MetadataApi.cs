//-----------------------------------------------------------------------
// <copyright file="MetadataApi.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;

namespace Experian.Qas.Updates.Metadata.WebApi.V1
{
    /// <summary>
    /// A class representing the default of implementation of <see cref="IMetadataApi"/> to access the QAS Electronic Updates Metadata API.
    /// </summary>
    [DebuggerDisplay("{ServiceUri}")]
    public class MetadataApi : IMetadataApi
    {
        /// <summary>
        /// The QAS Electronic Updates Metadata API URI. This field is read-only.
        /// </summary>
        private readonly Uri _serviceUri;

        /// <summary>
        /// The credentials to use to communicate with the QAS Electronic Updates Metadata API. This field is read-only.
        /// </summary>
        private readonly UserNamePassword _credentials = new UserNamePassword();

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataApi"/> class.
        /// </summary>
        /// <param name="serviceUri">The QAS Electronic Updates Metadata API service URI.</param>
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
        /// Gets the URI of the QAS Electronic Updates Metadata API.
        /// </summary>
        public Uri ServiceUri
        {
            get { return _serviceUri; }
        }

        /// <summary>
        /// Gets the user name to use to authenticate with the service.
        /// </summary>
        public string UserName
        {
            get { return _credentials.UserName; }
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
        public virtual async Task<AvailablePackagesReply> GetAvailablePackagesAsync()
        {
            GetAvailablePackagesRequest requestData = new GetAvailablePackagesRequest()
            {
                Credentials = _credentials,
            };

            try
            {
                return await Post<GetAvailablePackagesRequest, AvailablePackagesReply>("packages", requestData);
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
            FileDownloadRequest fileData = new FileDownloadRequest()
            {
                FileMD5Hash = fileHash,
                FileName = fileName,
                StartAtByte = startAtByte,
                EndAtByte = endAtByte,
            };

            GetDownloadUriRequest request = new GetDownloadUriRequest()
            {
                Credentials = _credentials,
                RequestData = fileData,
            };

            try
            {
                FileDownloadReply downloadResponse = await Post<GetDownloadUriRequest, FileDownloadReply>("filedownload", request);

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
        /// Sets the credentials to use to authenticate with the service.
        /// </summary>
        /// <param name="userName">The web service user name.</param>
        /// <param name="password">The web service password.</param>
        public virtual void SetCredentials(string userName, string password)
        {
            _credentials.UserName = userName;
            _credentials.Password = password;
        }

        /// <summary>
        /// Creates a new instance of <see cref="HttpClient"/> that can be used to consume
        /// the QAS Electronic Updates Metadata Web API.
        /// </summary>
        /// <returns>
        /// The created instance of <see cref="HttpClient"/>.
        /// </returns>
        protected virtual HttpClient CreateHttpClient()
        {
            var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
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
        /// <returns>
        /// A <see cref="Task{T}"/> that will yield an instance of <typeparamref name="TResult"/> read from the response as an asynchronous operation.
        /// </returns>
        protected virtual async Task<TResult> Post<TRequest, TResult>(string requestUri, TRequest value)
        {
            using (HttpClient client = CreateHttpClient())
            {
                MediaTypeFormatter formatter = CreateMediaTypeFormatter();

                using (HttpResponseMessage response = await client.PostAsync(requestUri, value, formatter))
                {
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsAsync<TResult>();
                }
            }
        }

        /// <summary>
        /// Creates a <see cref="MediaTypeFormatter"/> to use to serialize data.
        /// </summary>
        /// <returns>
        /// The created instance of <see cref="MediaTypeFormatter"/>.
        /// </returns>
        protected virtual MediaTypeFormatter CreateMediaTypeFormatter()
        {
            return new JsonMediaTypeFormatter();
        }
    }
}