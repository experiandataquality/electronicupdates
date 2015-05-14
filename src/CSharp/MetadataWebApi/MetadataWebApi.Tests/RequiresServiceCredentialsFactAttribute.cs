//-----------------------------------------------------------------------
// <copyright file="RequiresServiceCredentialsFactAttribute.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using Xunit;

namespace Experian.Qas.Updates.Metadata.WebApi.V1
{
    /// <summary>
    /// A test that requires service credentials to be configured as environment variables. This class cannot be inherited.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    internal sealed class RequiresServiceCredentialsFactAttribute : FactAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequiresServiceCredentialsFactAttribute"/> class.
        /// </summary>
        public RequiresServiceCredentialsFactAttribute()
            : base()
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("QAS_ElectronicUpdates_UserName")) ||
                string.IsNullOrEmpty(Environment.GetEnvironmentVariable("QAS_ElectronicUpdates_Password")))
            {
                this.Skip = "No service credentials are configured.";
            }
        }
    }
}