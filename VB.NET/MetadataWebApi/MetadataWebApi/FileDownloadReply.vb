' -----------------------------------------------------------------------
'  <copyright file="FileDownloadReply.vb" company="Experian Data Quality">
'   Copyright (c) Experian. All rights reserved.
'  </copyright>
' -----------------------------------------------------------------------

Imports System.Runtime.Serialization

''' <summary>
''' A class representing the details returned to the client when requesting to download a file.
''' </summary>
<DataContract(Namespace:="", Name:="FileDownloadReply")> _
Public Class FileDownloadReply

    ''' <summary>
    ''' Gets or sets the URI to download the file from.
    ''' </summary>
    <DataMember(Name:="DownloadUri")> _
    Public Property DownloadUri As String

End Class