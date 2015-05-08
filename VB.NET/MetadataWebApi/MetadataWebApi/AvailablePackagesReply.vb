' -----------------------------------------------------------------------
'  <copyright file="AvailablePackagesReply.vb" company="Experian Data Quality">
'   Copyright (c) Experian. All rights reserved.
'  </copyright>
' -----------------------------------------------------------------------

Imports System.Runtime.Serialization

''' <summary>
''' A class representing the data required by the client to download files.
''' </summary>
<DataContract(Namespace:="", Name:="AvailablePackagesReply")> _
Public Class AvailablePackagesReply

    ''' <summary>
    ''' Gets or sets the instances of the packages.
    ''' </summary>
    <DataMember(Name:="PackageGroups")> _
    Public Property PackageGroups As List(Of PackageGroup)

End Class