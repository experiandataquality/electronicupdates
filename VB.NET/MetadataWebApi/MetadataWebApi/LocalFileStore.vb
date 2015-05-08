' -----------------------------------------------------------------------
'  <copyright file="LocalFileStore.vb" company="Experian Data Quality">
'   Copyright (c) Experian. All rights reserved.
'  </copyright>
' -----------------------------------------------------------------------

Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Runtime.Serialization

''' <summary>
''' A class representing a file store that stores the available
''' data files in a serialized data file in the application directory.
''' This class cannot be inherited.
''' </summary>
Public NotInheritable Class LocalFileStore
    Implements IFileStore, IDisposable

    ''' <summary>
    ''' Initializes a new instance of the <see cref="LocalFileStore"/> class.
    ''' </summary>
    Public Sub New()

        If File.Exists(DataFileName) Then

            Dim serializer As New DataContractSerializer(GetType(Dictionary(Of String, String)))

            Using stream As Stream = File.OpenRead(DataFileName)

                Dim dictionary As IDictionary(Of String, String) = TryCast(serializer.ReadObject(stream), Dictionary(Of String, String))

                If (Not dictionary Is Nothing) Then

                    Dim pair As KeyValuePair(Of String, String)

                    For Each pair In dictionary
                        Me._fileStore.Item(pair.Key) = pair.Value
                    Next

                End If

            End Using

        End If

    End Sub

    ''' <summary>
    ''' Returns whether a file with the specified hash exists in the file store.
    ''' </summary>
    ''' <param name="hash">The hash of the data file to check for existence.</param>
    ''' <returns>
    ''' <see langword="True" /> if a file with the hash specified by <paramref name="hash" />
    ''' exists in the file store; otherwise <see langword="False" />.
    ''' </returns>
    Public Function ContainsFile(ByVal hash As String) As Boolean Implements IFileStore.ContainsFile
        Return _fileStore.ContainsKey(hash)
    End Function


    ''' <summary>
    ''' Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    ''' </summary>
    Public Sub Dispose() Implements IFileStore.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    ''' <summary>
    ''' Registers the specified file with the file store.
    ''' </summary>
    ''' <param name="hash">The hash of the file to register.</param>
    ''' <param name="path">The path of the file to register.</param>
    Public Sub RegisterFile(ByVal hash As String, ByVal path As String) Implements IFileStore.RegisterFile
        _fileStore.Item(hash) = System.IO.Path.GetFullPath(path)
    End Sub

    ''' <summary>
    ''' Attempts to retrieve the path of the file with the specified
    ''' hash from the file store.
    ''' </summary>
    ''' <param name="hash">The hash of the file to attempt to get the path of.</param>
    ''' <param name="path">When the method returns, contains the path of the file with the hash
    ''' specified by <paramref name="hash" /> if found; otherwise <see langword="Nothing" />.</param>
    ''' <returns>
    ''' <see langword="True" /> if a file with the hash specified by <paramref name="hash" />
    ''' was found in the file store; otherwise <see langword="False" />.
    ''' </returns>
    Public Function TryGetFilePath(ByVal hash As String, <Out> ByRef path As String) As Boolean Implements IFileStore.TryGetFilePath
        Return _fileStore.TryGetValue(hash, path)
    End Function

    ''' <summary>
    ''' Allows an object to try to free resources and perform other cleanup operations
    ''' before it is reclaimed by garbage collection.
    ''' </summary>
    Protected Overrides Sub Finalize()
        Try
            Dispose(False)
        Finally
            MyBase.Finalize()
        End Try
    End Sub

    ''' <summary>
    ''' Releases unmanaged and, optionally, managed resources.
    ''' </summary>
    ''' <param name="disposing">
    ''' <see langword="True" /> to release both managed and unmanaged resources;
    ''' <see langword="False" /> to release only unmanaged resources.
    ''' </param>
    Private Sub Dispose(ByVal disposing As Boolean)
        If Not Me._disposed Then

            If (disposing) Then
                ' Dispose of managed resources
            End If

            Dim serializer As New DataContractSerializer(GetType(Dictionary(Of String, String)))

            Using stream As Stream = File.Create(DataFileName)
                serializer.WriteObject(stream, _fileStore)
            End Using

            _disposed = True
        End If
    End Sub

    ''' <summary>
    ''' Whether the instance has been disposed.
    ''' </summary>
    Private _disposed As Boolean

    ''' <summary>
    ''' A dictionary containing the map of file hashes to paths.
    ''' </summary>
    Private ReadOnly _fileStore As IDictionary(Of String, String) = New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)

    ''' <summary>
    ''' Whether the instance has been disposed.
    ''' </summary>
    Private Const DataFileName As String = "FileStore.eu"

End Class