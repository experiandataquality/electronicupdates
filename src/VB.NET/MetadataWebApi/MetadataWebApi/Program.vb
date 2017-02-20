' -----------------------------------------------------------------------
'  <copyright file="Program.vb" company="Experian Data Quality">
'   Copyright (c) Experian. All rights reserved.
'  </copyright>
' -----------------------------------------------------------------------

Imports System.Configuration
Imports System.Globalization
Imports System.IO
Imports System.Net
Imports System.Reflection
Imports System.Security.Cryptography
Imports System.Text

''' <summary>
''' A class representing an example implementation of the QAS Electronic Updates
''' Metadata API.  This class cannot be inherited.
''' </summary>
Friend Class Program

    ''' <summary>
    ''' The main entry-point to the application.
    ''' </summary>
    Friend Shared Sub Main()

        PrintBanner()

        Try
            ' Get the configuration settings to connect to the QAS Electronic Updates Metadata API
            Dim token As String = GetAppSetting("Token")

            Dim downloadRootPath As String = "QASData"
            Dim verifyDownloads As Boolean = True

            If (String.IsNullOrEmpty(token)) Then
                Throw New ConfigurationErrorsException("Authentication token has not been configured.")
            End If

            Dim serviceUri As Uri = New Uri("https://ws.updates.qas.com/metadata/V2/")

            Console.WriteLine("QAS Electronic Updates Metadata REST API: {0}", serviceUri)
            Console.WriteLine()

            Dim service As IMetadataApi = New MetadataApi(serviceUri)

            ' Set the token used to authenticate with the service
            service.Token = token

            ' Query the packages available to the account
            Dim response As List(Of PackageGroup) = service.GetAvailablePackages()

            Console.WriteLine("Available Package Groups:")
            Console.WriteLine()

            ' Enumerate the package groups and list their packages and files
            If (Not response Is Nothing AndAlso response.Count > 0) Then

                Dim stopwatch As Stopwatch = Stopwatch.StartNew()

                ' Create a file store in which to cache information about which files
                ' have already been downloaded from the Metadata API service.
                Using fileStore As IFileStore = New LocalFileStore()

                    For Each group As PackageGroup In response

                        Console.WriteLine("Group Name: {0} ({1})", group.PackageGroupCode, group.Vintage)
                        Console.WriteLine()
                        Console.WriteLine("Packages:")
                        Console.WriteLine()

                        For Each package As Package In group.Packages

                            Console.WriteLine("Package Name: {0}", package.PackageCode)
                            Console.WriteLine()
                            Console.WriteLine("Files:")
                            Console.WriteLine()

                            For Each file As DataFile In package.Files

                                ' We already have this file, download not required
                                If fileStore.ContainsFile(file.MD5Hash) Then
                                    Console.WriteLine("File with hash '{0}' already downloaded.", file.MD5Hash)
                                Else

                                    ' Query the URIs to download the file from
                                    Dim uri As Uri = service.GetDownloadUri(
                                        file.MD5Hash,
                                        Nothing,
                                        Nothing)

                                    Console.WriteLine("File name: {0}", file.FileName)
                                    Console.WriteLine("File hash: {0}", file.MD5Hash)
                                    Console.WriteLine("File size: {0:N0}", file.Size)

                                    If (uri Is Nothing) Then
                                        Console.WriteLine("'{0}' is not available for download at this time.", file.FileName)
                                        Console.WriteLine()
                                    Else
                                        Console.WriteLine("File URI: {0}", uri)
                                        Console.WriteLine()

                                        ' Download the file and ensure the appropriate directories exist
                                        Using client As WebClient = New WebClient

                                            ' Create the path to the directory to download the file to
                                            Dim directoryPath As String = Path.Combine(
                                                downloadRootPath,
                                                group.PackageGroupCode,
                                                group.Vintage)

                                            Dim filePath As String = Path.GetFullPath(Path.Combine(directoryPath, file.FileName))

                                            ' Create the directory if it doesn't already exist
                                            If Not Directory.Exists(directoryPath) Then
                                                Directory.CreateDirectory(directoryPath)
                                            End If

                                            ' Download the file
                                            Console.WriteLine(
                                                "Downloading '{0}' ({1}) to '{2}'...",
                                                file.FileName,
                                                file.MD5Hash,
                                                filePath)

                                            client.DownloadFile(uri, filePath)

                                            ' Validate the download is correct, if configured
                                            If verifyDownloads And Not VerifyDownload(filePath, file.MD5Hash) Then
                                                ' Don't register the file in the file store as
                                                ' the file download became corrupted somehow
                                                Continue For
                                            End If

                                            ' Register the file with the file store so further invocations
                                            ' of the application don't unnecessarily download the file again
                                            fileStore.RegisterFile(file.MD5Hash, filePath)
                                        End Using
                                    End If
                                    Console.WriteLine()
                                End If
                                Console.WriteLine()
                            Next
                        Next
                        stopwatch.Stop()
                        Console.WriteLine("Downloaded data in {0:hh\:mm\:ss}.", stopwatch.Elapsed)
                        Console.WriteLine()
                    Next
                End Using

            End If

        Catch ex As Exception
            Console.WriteLine("Unhandled exception:")
            Console.WriteLine()
            Console.WriteLine(ex)
            Console.WriteLine()
        End Try

        Console.Write("Press any key to exit...")
        Console.ReadKey()

    End Sub

    ''' <summary>
    ''' Gets the configuration setting with the specified name.
    ''' </summary>
    ''' <param name="name">The name of the setting to get the value of.</param>
    ''' <returns>
    ''' The value of the setting specified by <paramref name="name"/>, if found; otherwise <see langword="null"/>.
    ''' </returns>
    Private Shared Function GetAppSetting(ByVal name As String) As String

        '' Build the full name of the setting
        Dim settingName As String = String.Format(CultureInfo.InvariantCulture, "EDQ:ElectronicUpdates:{0}", name)

        '' Is the setting configured in the application configuration file?
        Dim value As String = ConfigurationManager.AppSettings.Item(settingName)

        '' If Not, try the environment variables
        If (String.IsNullOrEmpty(value)) Then

            settingName = String.Format(CultureInfo.InvariantCulture, "EDQ_ElectronicUpdates_{0}", name)
            value = Environment.GetEnvironmentVariable(settingName)

        End If

        Return value

    End Function

    ''' <summary>
    ''' Validates that the specified file was downloaded correctly.
    ''' </summary>
    ''' <param name="filePath">The path of the downloaded file.</param>
    ''' <param name="expectedHash">The expected hash of <paramref name="filePath"/>.</param>
    ''' <returns>
    ''' <see langword="True"/> if the hash is correct; otherwise <see langword="False"/>.
    ''' </returns>
    Private Shared Function VerifyDownload(ByVal filePath As String, ByVal expectedHash As String) As Boolean

        Console.WriteLine("Validating hash of '{0}'...", filePath)

        Dim isHashCorrect As Boolean

        filePath = Path.GetFullPath(filePath)

        Using algorithm As HashAlgorithm = New MD5CryptoServiceProvider
            Using stream As Stream = File.OpenRead(filePath)

                Dim hash As Byte() = algorithm.ComputeHash(stream)
                Dim builder As New StringBuilder

                For Each b As Byte In hash
                    builder.AppendFormat(CultureInfo.InvariantCulture, "{0:x2}", b)
                Next

                isHashCorrect = String.Equals(builder.ToString, expectedHash, StringComparison.Ordinal)

                If Not isHashCorrect Then
                    Console.WriteLine("The MD5 hash of '{0}' ({1}) does not match the expected hash of '{2}'.  A HTTP transmission error is likely to have occurred.", filePath, builder, expectedHash)
                End If

            End Using
        End Using

        Console.WriteLine()
        Return isHashCorrect

    End Function

    ''' <summary>
    ''' Prints the application banner to the console.
    ''' </summary>
    Private Shared Sub PrintBanner()

        Dim assembly As Assembly = assembly.GetExecutingAssembly
        Dim currentAssembly As AssemblyName = assembly.GetName()

        Dim copyright As AssemblyCopyrightAttribute = TryCast(assembly.GetCustomAttributes(GetType(AssemblyCopyrightAttribute), False)(0), AssemblyCopyrightAttribute)
        Dim version As AssemblyInformationalVersionAttribute = TryCast(assembly.GetCustomAttributes(GetType(AssemblyInformationalVersionAttribute), False)(0), AssemblyInformationalVersionAttribute)

        Dim message As String = String.Format(
            CultureInfo.InvariantCulture,
            "{0}{1} (v{2}) - {3}{0}",
            Environment.NewLine,
            currentAssembly.Name,
            version.InformationalVersion,
            copyright.Copyright)

        Console.WriteLine(message)

    End Sub

End Class