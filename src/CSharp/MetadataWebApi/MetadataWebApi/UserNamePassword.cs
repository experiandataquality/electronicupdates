//-----------------------------------------------------------------------
// <copyright file="UserNamePassword.cs" company="Experian Data Quality">
//   Copyright (c) Experian. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Runtime.Serialization;

namespace Experian.Qas.Updates.Metadata.WebApi.V1
{
    /// <summary>
    /// A class representing a user's credentials.
    /// </summary>
    [DataContract(Namespace = "", Name = "UserNamePassword")]
    public class UserNamePassword
    {
        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        [DataMember(Name = "UserName", IsRequired = true, Order = 1)]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the plaintext password.
        /// </summary>
        [DataMember(Name = "Password", IsRequired = true, Order = 2)]
        public string Password { get; set; }
    }
}