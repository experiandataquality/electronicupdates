//-----------------------------------------------------------------------
// <copyright file="GlobalSuppressions.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Experian.Qas.Updates.Metadata.WebApi.V2.AvailablePackagesReply.#PackageGroups", Justification = "Required for serialization.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Experian.Qas.Updates.Metadata.WebApi.V2.Package.#Files", Justification = "Required for serialization.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "Experian.Qas.Updates.Metadata.WebApi.V2.PackageGroup.#Packages", Justification = "Required for serialization.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Experian.Qas.Updates.Metadata.WebApi.V2.IMetadataApi.#GetAvailablePackagesAsync()", Justification = "Requires a web service call.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#", Scope = "member", Target = "Experian.Qas.Updates.Metadata.WebApi.V2.MetadataApi.#Post`2(System.String,!!0)", Justification = "Matches the Web API infrastructure.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Scope = "member", Target = "Experian.Qas.Updates.Metadata.WebApi.V2.FileDownloadReply.#DownloadUri", Justification = "Matches the Web API infrastructure.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "hh", Scope = "member", Target = "Experian.Qas.Updates.Metadata.WebApi.V2.Program.#MainInternal()", Justification = "Value is a format string.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ss", Scope = "member", Target = "Experian.Qas.Updates.Metadata.WebApi.V2.Program.#MainInternal()", Justification = "Value is a format string.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Scope = "member", Target = "Experian.Qas.Updates.Metadata.WebApi.V2.AvailablePackagesReply.#PackageGroups", Justification = "Required for serialization.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Scope = "member", Target = "Experian.Qas.Updates.Metadata.WebApi.V2.Package.#Files", Justification = "Required for serialization.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Scope = "member", Target = "Experian.Qas.Updates.Metadata.WebApi.V2.PackageGroup.#Packages", Justification = "Required for serialization.")]
