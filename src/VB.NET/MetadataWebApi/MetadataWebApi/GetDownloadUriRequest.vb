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
    ''' Gets or sets the data for the file to request the download URL for.
    ''' </summary>
    <DataMember(Name:="fileDownloadRequest", IsRequired:=True, Order:=1)>
    Public Property RequestData As FileDownloadRequest

End Class