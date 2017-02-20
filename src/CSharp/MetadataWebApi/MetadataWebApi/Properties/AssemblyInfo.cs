//-----------------------------------------------------------------------
// <copyright file="AssemblyInfo.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyDescription("Electronic Updates Metadata REST API sample application.")]
[assembly: AssemblyTitle("MetadataWebApi")]

[assembly: CLSCompliant(true)]

[assembly: Guid("d5dc8a13-ac4d-484e-b183-8b64b951efeb")]

#if UNIX
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("MetadataWebApi.Tests")]
#else
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2,PublicKey=0024000004800000940000000602000000240000525341310004000001000100d99b4a93e03542054a6d5f4ef5914b432f2c28cb218211daef37c7b837d747c46b1675236a9bd7170dac2271683980b34270da98bdcb93e713b522385065841cc4f4e4eace77c39d75bd9e035cc97afa0d0ed7c9f335fae24faca74313a56fb60d0286408677c6ed53b5fa2d282777599f71fd4eca040cca9ab916a0e2e855ca")]
[assembly: InternalsVisibleTo("MetadataWebApi.Tests,PublicKey=0024000004800000940000000602000000240000525341310004000001000100d99b4a93e03542054a6d5f4ef5914b432f2c28cb218211daef37c7b837d747c46b1675236a9bd7170dac2271683980b34270da98bdcb93e713b522385065841cc4f4e4eace77c39d75bd9e035cc97afa0d0ed7c9f335fae24faca74313a56fb60d0286408677c6ed53b5fa2d282777599f71fd4eca040cca9ab916a0e2e855ca")]
#endif
