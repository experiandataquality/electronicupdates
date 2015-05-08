' --------------------------------------------------------------------------
'  <copyright file="GetAvailablePackagesRequest.vb" company="Experian Data Quality">
'   Copyright (c) Experian. All rights reserved.
'  </copyright>
' --------------------------------------------------------------------------

Imports System.Runtime.Serialization

''' <summary>
''' A class representing a request for the available packages.
''' </summary>
<DataContract(Name:="GetAvailablePackages", Namespace:="")> _
Public Class GetAvailablePackagesRequest

    ''' <summary>
    ''' Gets or sets the credentials for authenticating with the web service.
    ''' </summary>
    <DataMember(Name:="usernamePassword", IsRequired:=True)> _
    Public Property Credentials As UserNamePassword

End Class