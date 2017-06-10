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
        void Create(UserModel userModel);

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
        UserModel Get(string userName);

        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="userPassword">new password</param>
        void ChangePassword(string userName, string userPassword);

        /// <summary>
        /// Check if user exists
        /// </summary>
        /// <param name="userName">user name</param>
        /// <returns></returns>
        bool Exists(string userName);

        /// <summary>
        /// Change if password is required on user logon
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="value">flag value</param>
        /// <returns></returns>
        void SetPasswordRequired(string userName, bool value);

        /// <summary>
        /// Set if password can be changed by user
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="value">flag value</param>
        /// <returns></returns>
        void SetPasswordCanBeChangedByUser(string userName, bool value);

        /// <summary>
        /// When set, the password will not expire for this account
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="value">flag value</param>
        /// <returns></returns>
        void SetPasswordCanExpire(string userName, bool value);

        ///// <summary>
        ///// Set if password has been expired or not
        ///// </summary>
        ///// <param name="userName">user name</param>
        ///// <param name="value">flag value</param>
        ///// <returns></returns>
        //void SetPasswordExpired(string userName, bool value);
    }
}