' -----------------------------------------------------------------------
'  <copyright file="Package.vb" company="Experian Data Quality">
'   Copyright (c) Experian. All rights reserved.
'  </copyright>
' -----------------------------------------------------------------------

Imports System.Runtime.Serialization

''' <summary>
''' A class representing an instance of a package.
''' </summary>
<DataContract(Namespace:="", Name:="Package")> _
<DebuggerDisplay("{PackageCode}")> _
Public Class Package

    ''' <summary>
    ''' Gets or sets the package code.
    ''' </summary>
    <DataMember(Name:="PackageCode")> _
    Public Property PackageCode As String

    ''' <summary>
    ''' Gets or sets the list of the files associated with this package.
    ''' </summary>
    <DataMember(Name:="Files")> _
    Public Property Files As List(Of DataFile)

End Class