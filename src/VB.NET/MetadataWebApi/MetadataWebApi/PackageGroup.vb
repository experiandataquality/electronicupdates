' -----------------------------------------------------------------------
'  <copyright file="PackageGroup.vb" company="Experian Data Quality">
'   Copyright (c) Experian. All rights reserved.
'  </copyright>
' -----------------------------------------------------------------------

Imports System.Runtime.Serialization

''' <summary>
''' A class representing a single vintage of data.
''' </summary>
<DataContract(Namespace:="", Name:="PackageGroup")> _
<DebuggerDisplay("{PackageGroupCode} {Vintage}")> _
Public Class PackageGroup

    ''' <summary>
    ''' Gets or sets the package group code.
    ''' </summary>
    <DataMember(Name:="PackageGroupCode")> _
    Public Property PackageGroupCode As String

    ''' <summary>
    ''' Gets or sets the dataset vintage.
    ''' </summary>
    <DataMember(Name:="Vintage")> _
    Public Property Vintage As String

    ''' <summary>
    ''' Gets or sets the individual packages of this group.
    ''' </summary>
    <DataMember(Name:="Packages")> _
    Public Property Packages As List(Of Package)

End Class