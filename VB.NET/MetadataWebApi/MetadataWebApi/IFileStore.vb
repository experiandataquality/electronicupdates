' -----------------------------------------------------------------------
'  <copyright file="IFileStore.vb" company="Experian Data Quality">
'   Copyright (c) Experian. All rights reserved.
'  </copyright>
' -----------------------------------------------------------------------

Imports System.Runtime.InteropServices

''' <summary>
''' Defines a file store of data files.
''' </summary>
Public Interface IFileStore
    Inherits IDisposable

    ''' <summary>
    ''' Returns whether a file with the specified hash exists in the file store.
    ''' </summary>
    ''' <param name="hash">The hash of the data file to check for existence.</param>
    ''' <returns>
    ''' <see langword="True"/> if a file with the hash specified by <paramref name="hash"/>
    ''' exists in the file store; otherwise <see langword="False"/>.
    ''' </returns>
    Function ContainsFile(ByVal hash As String) As Boolean

    ''' <summary>
    ''' Registers the specified file with the file store.
    ''' </summary>
    ''' <param name="hash">The hash of the file to register.</param>
    ''' <param name="path">The path of the file to register.</param>
    Sub RegisterFile(ByVal hash As String, ByVal path As String)

    ''' <summary>
    ''' Attempts to retrieve the path of the file with the specified
    ''' hash from the file store.
    ''' </summary>
    ''' <param name="hash">The hash of the file to attempt to get the path of.</param>
    ''' <param name="path">
    ''' When the method returns, contains the path of the file with the hash
    ''' specified by <paramref name="hash"/> if found; otherwise <see langword="Nothing"/>.
    ''' </param>
    ''' <returns>
    ''' <see langword="True"/> if a file with the hash specified by <paramref name="hash"/>
    ''' was found in the file store; otherwise <see langword="False"/>.
    ''' </returns>
    Function TryGetFilePath(ByVal hash As String, <Out> ByRef path As String) As Boolean

End Interface