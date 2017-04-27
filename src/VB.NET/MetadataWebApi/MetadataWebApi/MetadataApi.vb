' -----------------------------------------------------------------------
'  <copyright file="MetadataApiBase.vb" company="Experian Data Quality">
'   Copyright (c) Experian. All rights reserved.
'  </copyright>
' -----------------------------------------------------------------------

Imports System.Globalization
Imports System.Net.Http
Imports System.Net.Http.Formatting
Imports System.Net.Http.Headers
Imports System.Reflection

''' <summary>
''' A class representing the default implementation of <see cref="IMetadataApi"/> to access the Experian Data Quality Electronic Updates Metadata API.
''' </summary>
<DebuggerDisplay("{ServiceUri}")>
Public Class MetadataApi
    Implements IMetadataApi

    Private _token As String

    ''' <summary>
    ''' Initializes a new instance of the <see cref="MetadataApi"/> class.
    ''' </summary>
    ''' <param name="serviceUri">The Electronic Updates Metadata API service URI.</param>
    ''' <exception cref="ArgumentNullException">
    ''' <paramref name="serviceUri"/> is <see langword="Nothing"/>.
    ''' </exception>
    Public Sub New(ByVal serviceUri As Uri)

        If (serviceUri Is Nothing) Then
            Throw New ArgumentNullException("serviceUri")
        End If

        _serviceUri = serviceUri

    End Sub

    ''' <summary>
    ''' Returns the available updates packages.
    ''' </summary>
    ''' <returns>
    ''' The available updates packages.
    ''' </returns>
    ''' <exception cref="MetadataApiException">
    ''' The available packages could not be retrieved.
    ''' </exception>
    Public Function GetAvailablePackages() As List(Of PackageGroup) Implements IMetadataApi.GetAvailablePackages

        Try
            Using client As HttpClient = Me.CreateHttpClient

                Dim formatter As MediaTypeFormatter = Me.CreateMediaTypeFormatter

                Dim tokenHeader As String = String.Format(CultureInfo.InvariantCulture, "x-api-key {0}", Token)

                client.DefaultRequestHeaders.Add("Authorization", tokenHeader)

                Using response As HttpResponseMessage = client.GetAsync("packages").Result
                    response.EnsureSuccessStatusCode()
                    Return response.Content.ReadAsAsync(Of List(Of PackageGroup))().Result
                End Using
            End Using
        Catch ex As Exception
            Throw New MetadataApiException(ex.Message, ex)
        End Try

    End Function

    ''' <summary>
    ''' Gets the download <see cref="Uri"/> for the specified file.
    ''' </summary>
    ''' <param name="fileName">The name of the file to download.</param>
    ''' <param name="fileHash">The hash of the file to download.</param>
    ''' <param name="startAtByte">The byte to start downloading the file from.</param>
    ''' <param name="endAtByte">The byte to end downloading the file from.</param>
    ''' <returns>
    ''' The <see cref="Uri"/> to download the file specified by <paramref name="fileName"/>
    ''' and <paramref name="fileHash"/> from or <see langword="Nothing"/> if the file is not available.
    ''' </returns>
    ''' <exception cref="MetadataApiException">
    ''' The download URI could not be obtained.
    ''' </exception>
    Public Function GetDownloadUri(fileName As String, fileHash As String, startAtByte As Long?, endAtByte As Long?) As Uri Implements IMetadataApi.GetDownloadUri

        Dim request As New GetDownloadUriRequest With {
            .endAtByte = endAtByte,
            .FileMD5Hash = fileHash,
            .fileName = fileName,
            .startAtByte = startAtByte
        }

        Try
            Dim downloadResponse As FileDownloadReply = Me.Post(Of GetDownloadUriRequest, FileDownloadReply)("filelink", request, Token)

            If (downloadResponse Is Nothing Or downloadResponse.DownloadUri Is Nothing) Then
                Return Nothing
            End If

            Return New Uri(downloadResponse.DownloadUri)
        Catch ex As Exception
            Throw New MetadataApiException(ex.Message, ex)
        End Try

    End Function

    ''' <summary>
    ''' Creates a new instance of <see cref="HttpClient"/> that can be used to consume
    ''' the Electronic Updates Metadata Web API.
    ''' </summary>
    ''' <returns>
    ''' The created instance of <see cref="HttpClient"/>.
    ''' </returns>
    Protected Overridable Function CreateHttpClient() As HttpClient

        Dim assembly As Assembly = Assembly.GetEntryAssembly

        If (assembly Is Nothing) Then
            assembly = Assembly.GetExecutingAssembly
        End If

        Dim assemblyName As AssemblyName = assembly.GetName

        Dim contentTypeHeader As New MediaTypeWithQualityHeaderValue(Me.ContentType)
        Dim userAgentHeader As New ProductInfoHeaderValue(assemblyName.Name, assemblyName.Version.ToString)

        Dim client As New HttpClient

        Try
            client.BaseAddress = _serviceUri

            ' Add any other default and/or custom HTTP request headers here
            client.DefaultRequestHeaders.Accept.Add(contentTypeHeader)
            client.DefaultRequestHeaders.UserAgent.Add(userAgentHeader)

            Return client
        Catch ex As Exception
            client.Dispose()
            Throw
        End Try

    End Function

    ''' <summary>
    ''' Posts the specified value to the specified relative URI and reads the response.
    ''' </summary>
    ''' <typeparam name="TRequest">The type of the request.</typeparam>
    ''' <typeparam name="TResult">The type of the response.</typeparam>
    ''' <param name="requestUri">The relative URI the request is sent to.</param>
    ''' <param name="value">The value to write into the entity body of the request.</param>
    ''' <param name="token">The authentication token.</param>
    ''' <returns>
    ''' An instance of <typeparamref name="TResult"/> read from the response.
    ''' </returns>
    Protected Overridable Function Post(Of TRequest, TResult)(ByVal requestUri As String, ByVal value As TRequest, ByVal token As String) As TResult
        Using client As HttpClient = Me.CreateHttpClient

            Dim formatter As MediaTypeFormatter = Me.CreateMediaTypeFormatter

            Dim tokenHeader As String = String.Format(CultureInfo.InvariantCulture, "x-api-key {0}", token)

            client.DefaultRequestHeaders.Add("Authorization", tokenHeader)

            Using response As HttpResponseMessage = client.PostAsync(requestUri, value, formatter).Result
                response.EnsureSuccessStatusCode()
                Return response.Content.ReadAsAsync(Of TResult)().Result
            End Using
        End Using
    End Function

    ''' <summary>
    ''' When implemented in a derived class, creates a <see cref="MediaTypeFormatter"/> to use to serialize data.
    ''' </summary>
    ''' <returns>
    ''' The created instance of <see cref="MediaTypeFormatter"/>.
    ''' </returns>
    Protected Overridable Function CreateMediaTypeFormatter() As MediaTypeFormatter
        Return New JsonMediaTypeFormatter
    End Function

    ''' <summary>
    ''' Gets the URI of the Electronic Updates Metadata API.
    ''' </summary>
    Public ReadOnly Property ServiceUri As Uri
        Get
            Return _serviceUri
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets the user name to use to authenticate with the service.
    ''' </summary>
    Public Property Token As String Implements IMetadataApi.Token
        Get
            Return _token
        End Get
        Set(ByVal value As String)
            _token = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the HTTP request content type.
    ''' </summary>
    Protected Overridable ReadOnly Property ContentType As String
        Get
            Return "application/json"
        End Get
    End Property

    ''' <summary>
    ''' The Electronic Updates Metadata API URI.
    ''' </summary>
    Private ReadOnly _serviceUri As Uri

End Class