using System;
using System.Collections.Generic;
using System.Security;
using Continuous.User.Users.Model;

namespace Continuous.User.Users
{
    /// <summary>
    /// Shell for managing local users accounts
    /// </summary>
    public interface IUserShell
    {
        /// <summary>
        /// Create new user account
        /// </summary>
        /// <param name="userModel">user model</param>
        [Obsolete("Use create with LocalUserCreateModel as input parameter")]
        void Create(UserModel userModel);

        /// <summary>
        /// Create new user account
        /// </summary>
        /// <param name="model"></param>
        void Create(LocalUserCreateModel model);

        /// <summary>
        /// Remove user account
        /// </summary>
        /// <param name="userName">user name</param>
        void Remove(string userName);

        /// <summary>
        /// Get user account
        /// </summary>
        /// <param name="userName">user name</param>
        /// <returns></returns>
        [Obsolete("Use GetLocalUser")]
        UserModel Get(string userName);

        /// <summary>
        /// Get local user account info
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        LocalUserInfo GetLocalUser(string userName);

        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="userPassword">new password</param>
        void ChangePassword(string userName, string userPassword);

        /// <summary>
        /// Change user password with more secure password parameter
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        void ChangePassword(string userName, SecureString userPassword);

        /// <summary>
        /// Check if user exists
        /// </summary>
        /// <param name="userName">user name</param>
        /// <returns></returns>
        bool Exists(string userName);

        /// <summary>
        /// Specify wheter password is required at user logon
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="value">new property value</param>
        /// <returns></returns>
        void SetPasswordRequired(string userName, bool value);

        /// <summary>
        /// Specify wheter the password can be changed by user
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="value">new property value</param>
        /// <returns></returns>
        void SetPasswordCanBeChangedByUser(string userName, bool value);

        /// <summary>
        /// Specify wheter the password can expire
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="value">new property value</param>
        /// <returns></returns>
        void SetPasswordCanExpire(string userName, bool value);

        /// <summary>
        /// Specify wheter password has been expired
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="value">property value</param>
        /// <returns></returns>
        void SetPasswordExpired(string userName, bool value);

        /// <summary>
        /// Specify wheter account has been disabled
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="value">new property value</param>
        void SetAccountDisabled(string userName, bool value);

        /// <summary>
        /// Specify wheter user is visible in windows welcome screen
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="value">new property value</param>
        void SetUserVisibility(string userName, bool value);

        /// <summary>
        /// Change user description
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="description"></param>
        void SetUserDescription(string userName, string description);

        /// <summary>
        /// Change user full name
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="fullName"></param>
        void SetUserFullName(string userName, string fullName);

        /// <summary>
        /// Specify when user account should expire - [null] if never. 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="dateTime"></param>
        void SetAccountExpirationDate(string userName, DateTime? dateTime);
            
        /// <summary>
        /// Get currently logged-in user
        /// </summary>
        LocalUserInfo GetLoggedInUser();

        /// <summary>
        /// Get all users from current domain 
        /// </summary>
        /// <returns></returns>
        List<LocalUserInfo> GetAllUsers();
    }
}