' -----------------------------------------------------------------------
'  <copyright file="MetadataApiException.vb" company="Experian Data Quality">
'   Copyright (c) Experian. All rights reserved.
'  </copyright>
' -----------------------------------------------------------------------

Imports System.Runtime.Serialization

''' <summary>
''' Represents error data when an error is returned by the QAS Electronic Updates Metadata API.
''' </summary>
<Serializable> _
Public Class MetadataApiException
    Inherits Exception

    ''' <summary>
    ''' Initializes a new instance of the <see cref="MetadataApiException"/> class.
    ''' </summary>
    Public Sub New()
        MyBase.New("An error was returned by the QAS Electronic Updates Metadata API.")
    End Sub

    ''' <summary>
    ''' Initializes a new instance of the <see cref="MetadataApiException"/> class
    ''' with a specified error message.
    ''' </summary>
    ''' <param name="message">The message that describes the error.</param>
    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    ''' <summary>
    ''' Initializes a new instance of the <see cref="MetadataApiException"/> class with a
    ''' specified error message and a reference to the inner exception that is the cause of this exception.
    ''' </summary>
    ''' <param name="message">The error message that explains the reason for the exception.</param>
    ''' <param name="innerException">
    ''' The exception that is the cause of the current exception, or <see langword="Nothing"/>
    ''' if no inner exception is specified.
    ''' </param>
    Public Sub New(ByVal message As String, ByVal innerException As Exception)
        MyBase.New(message, innerException)
    End Sub

    ''' <summary>
    ''' Initializes a new instance of the <see cref="MetadataApiException"/> class.
    ''' </summary>
    ''' <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
    ''' <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
    ''' <exception cref="ArgumentNullException">
    ''' The <paramref name="info"/> parameter is <see langword="Nothing"/>.
    ''' </exception>
    Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        MyBase.New(info, context)
    End Sub

End Class