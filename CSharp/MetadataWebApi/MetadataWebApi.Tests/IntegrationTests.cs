//-----------------------------------------------------------------------
// <copyright file="IntegrationTests.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Globalization;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Experian.Qas.Updates.Metadata.WebApi.V1
{
    public class IntegrationTests
    {
        private readonly ITestOutputHelper _output;

        public IntegrationTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [RequiresServiceCredentialsFact]
        public void Program_Downloads_Data_Files()
        {
            // Arrange
            using (TextWriter writer = new StringWriter(CultureInfo.InvariantCulture))
            {
                Console.SetOut(writer);

                try
                {
                    // Act
                    Program.MainInternal();

                    // Assert
                    Assert.True(Directory.Exists("QASData"));
                    Assert.NotEmpty(Directory.GetFiles("QASData", "*", SearchOption.AllDirectories));
                }
                finally
                {
                    _output.WriteLine(writer.ToString());
                }
            }
        }

        private sealed class RequiresServiceCredentialsFact : FactAttribute
        {
            public RequiresServiceCredentialsFact()
                : base()
            {
                if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("QAS:ElectronicUpdates:UserName")) ||
                    string.IsNullOrEmpty(Environment.GetEnvironmentVariable("QAS:ElectronicUpdates:Password")))
                {
                    this.Skip = "No service credentials are configured.";
                }
            }
        }
    }
}