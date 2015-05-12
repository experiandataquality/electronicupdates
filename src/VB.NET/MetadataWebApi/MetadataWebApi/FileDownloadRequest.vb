' -----------------------------------------------------------------------
'  <copyright file="FileDownloadRequest.vb" company="Experian Data Quality">
'   Copyright (c) Experian. All rights reserved.
'  </copyright>
' -----------------------------------------------------------------------

Imports System.Runtime.Serialization

''' <summary>
''' A class representing details of a request to download a file.
''' </summary>
<DataContract(Namespace:="", Name:="FileDownloadRequest")> _
Public Class FileDownloadRequest

    ''' <summary>
    ''' Gets or sets the MD5 of the file requested to be downloaded.
    ''' </summary>
    <DataMember(Name:="FileMd5Hash", IsRequired:=True)> _
    Public Property FileMD5Hash As String

    ''' <summary>
    ''' Gets or sets the name of the file requested to be downloaded.
    ''' </summary>
    <DataMember(Name:="FileName", IsRequired:=True)> _
    Public Property FileName As String

    ''' <summary>
    ''' Gets or sets the byte to start downloading from, if any.
    ''' </summary>
    <DataMember(Name:="StartAtByte", IsRequired:=False)> _
    Public Property StartAtByte As Long?

    ''' <summary>
    ''' Gets or sets the byte to end downloading at, if any.
    ''' </summary>
    <DataMember(Name:="EndAtByte", IsRequired:=False)> _
    Public Property EndAtByte As Long?

End Class