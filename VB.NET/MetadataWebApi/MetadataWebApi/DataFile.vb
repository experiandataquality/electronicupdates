' -----------------------------------------------------------------------
'  <copyright file="DataFile.vb" company="Experian Data Quality">
'   Copyright (c) Experian. All rights reserved.
'  </copyright>
' -----------------------------------------------------------------------

Imports System.Runtime.Serialization

''' <summary>
''' A class representing a single data file.
''' </summary>
<DataContract(Namespace:="", Name:="DataFile")> _
<DebuggerDisplay("{FileName} {MD5Hash}")> _
Public Class DataFile

    ''' <summary>
    ''' Gets or sets the file name.
    ''' </summary>
    <DataMember(Name:="FileName")> _
    Public Property FileName As String

    ''' <summary>
    ''' Gets or sets the MD5 hash of the file.
    ''' </summary>    
    <DataMember(Name:="Md5Hash")> _
    Public Property MD5Hash As String

    ''' <summary>
    ''' Gets or sets the size of the file in bytes.
    ''' </summary>
    <DataMember(Name:="Size")> _
    Public Property Size As Long

End Class