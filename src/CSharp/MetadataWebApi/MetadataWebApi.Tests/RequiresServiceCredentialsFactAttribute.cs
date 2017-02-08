//-----------------------------------------------------------------------
// <copyright file="RequiresServiceCredentialsFactAttribute.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using Xunit;

namespace Experian.Qas.Updates.Metadata.WebApi.V2
{
    /// <summary>
    /// A test that requires the authentication token to be configured as an environment variable. This class cannot be inherited.
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
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("QAS_ElectronicUpdates_Token")))
            {
                this.Skip = "Authentication token has not been configured.";
            }
        }
    }
}
