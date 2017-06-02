using System.Collections.Generic;

namespace Continuous.User.LocalUserGroups
{
    /// <summary>
    /// Shell to manage local users groups 
    /// </summary>
    public interface ILocalUserGroupShell
    {
        /// <summary>
        /// Create new local user group
        /// </summary>
        /// <param name="name">group name</param>
        /// <param name="description">group description</param>
        void Create(string name, string description);

        /// <summary>
        /// Remove existing local user group
        /// </summary>
        /// <param name="groupName">group name</param>
        void Remove(string groupName);

        /// <summary>
        /// Get existing local user group
        /// </summary>
        /// <param name="groupName">group name</param>
        /// <returns></returns>
        Model.LocalUserGroup Get(string groupName);

        /// <summary>
        /// Assign existing users to group
        /// </summary>
        /// <param name="groupName">group name</param>
        /// <param name="userNames">list of users</param>
        void AssignUsers(string groupName, List<string> userNames);

        /// <summary>
        /// Remove users from group
        /// </summary>
        /// <param name="groupName">group name</param>
        /// <param name="userNames">list of users</param>
        void RemoveUsers(string groupName, List<string> userNames);
    }
}