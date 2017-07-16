using System;

namespace Continuous.User.Users.Model
{
    /// <summary>
    /// Model for creating new local user
    /// </summary>
    public class LocalUserCreateModel
    {
        /// <summary>
        /// User name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User full name
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// User description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// User account expiration date - if null, the account will never exipre
        /// </summary>
        public DateTime? AccountExpires { get; set; }

        /// <summary>
        /// User Password - will be empty for getUser
        /// </summary>
        public string Password { get; set; }
    }
}
