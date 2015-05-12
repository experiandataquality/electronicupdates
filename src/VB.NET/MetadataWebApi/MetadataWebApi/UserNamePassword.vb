' -----------------------------------------------------------------------
'  <copyright file="UserNamePassword.vb" company="Experian Data Quality">
'   Copyright (c) Experian. All rights reserved.
'  </copyright>
' -----------------------------------------------------------------------

Imports System.Runtime.Serialization

''' <summary>
''' A class representing a user's credentials.
''' </summary>
<DataContract(Namespace:="", Name:="UserNamePassword")> _
Public Class UserNamePassword

    ''' <summary>
    ''' Gets or sets the user name.
    ''' </summary>
    <DataMember(Name:="UserName", IsRequired:=True, Order:=1)> _
    Public Property UserName As String

    ''' <summary>
    ''' Gets or sets the plaintext password.
    ''' </summary>
    <DataMember(Name:="Password", IsRequired:=True, Order:=2)> _
    Public Property Password As String

End Class
