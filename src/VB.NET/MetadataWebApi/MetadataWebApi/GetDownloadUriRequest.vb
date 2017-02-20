' -----------------------------------------------------------------------
'  <copyright file="GetDownloadUriRequest.vb" company="Experian Data Quality">
'   Copyright (c) Experian. All rights reserved.
'  </copyright>
' -----------------------------------------------------------------------

Imports System.Runtime.Serialization

''' <summary>
''' A class representing a request to obtain the download URI for a data file.
''' </summary>
<DataContract(Name:="GetFileDownload", Namespace:="")>
Public Class GetDownloadUriRequest

    ''' <summary>
    ''' Gets or sets the MD5 of the file requested to be downloaded.
    ''' </summary>
    <DataMember(Name:="FileMd5Hash", IsRequired:=True)>
    Public Property FileMD5Hash As String

    ''' <summary>
    ''' Gets or sets the byte to start downloading from, if any.
    ''' </summary>
    <DataMember(Name:="StartAtByte", IsRequired:=False)>
    Public Property StartAtByte As Long?

    ''' <summary>
    ''' Gets or sets the byte to end downloading at, if any.
    ''' </summary>
    <DataMember(Name:="EndAtByte", IsRequired:=False)>
    Public Property EndAtByte As Long?

End Class