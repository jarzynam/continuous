using System.Collections.Generic;

namespace Continuous.User.LocalUserGroups.Model
{
    /// <summary>
    /// Local user group instance
    /// </summary>
    public class LocalUserGroup
    {
        /// <summary>
        /// group name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// group description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// list of users assigned to this group
        /// </summary>
        public List<string> Members { get; set; }
    }
}
