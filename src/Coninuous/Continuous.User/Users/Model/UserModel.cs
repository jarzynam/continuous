using System;

namespace Continuous.User.Users.Model
{
    /// <summary>
    /// User representation 
    /// </summary>
    public class UserModel
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
        /// User Password - will be empty for getUser
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// User accout expiration date - if null, the account will never exipre
        /// </summary>
        public DateTime? Expires { get; set; }
    }
}
