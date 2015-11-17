//-----------------------------------------------------------------------
// <copyright file="UserNamePassword.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Newtonsoft.Json;

namespace Experian.Qas.Updates.Metadata.WebApi.V1
{
    /// <summary>
    /// A class representing a user's credentials.
    /// </summary>
    public class UserNamePassword
    {
        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        [JsonProperty("UserName")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the plaintext password.
        /// </summary>
        [JsonProperty("Password")]
        public string Password { get; set; }
    }
}
