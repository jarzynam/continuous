using System.Security;

namespace Continuous.User.Users.Extensions.LocalUserInfo
{
    /// <summary>
    /// Fast updating local user info properties directly from the class
    /// </summary>
    public interface ILocalUserInfoUpdate
    {
        
        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        ILocalUserInfoUpdate Password(SecureString newPassword);

        /// <summary>
        /// Specify wheter password is required at user logon
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
        ILocalUserInfoUpdate PasswordRequired(bool newValue);

        /// <summary>
        /// Specify wheter the password can be changed by user
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
        ILocalUserInfoUpdate PasswordCanBeChangedByUser(bool newValue);

        /// <summary>
        /// Specify wheter the password can expire
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
        ILocalUserInfoUpdate PasswordCanExpire(bool newValue);

        /// <summary>
        /// Specify wheter password has been expired
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
        ILocalUserInfoUpdate PasswordExpired(bool newValue);

        /// <summary>
        /// Specify wheter account has been disabled
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
        ILocalUserInfoUpdate AccountDisabled(bool newValue);

        /// <summary>
        /// Specify wheter user is visible in windows welcome screen
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
        ILocalUserInfoUpdate IsVisible(bool newValue);

        /// <summary>
        /// Rollback all properties except user password when error occur.
        /// </summary>
        /// <returns></returns>
        ILocalUserInfoUpdate RollbackOnError();

        /// <summary>
        /// Cancel updating proccess. No changes will be made.
        /// </summary>
        /// <returns></returns>
        Model.LocalUserInfo Cancel();

        /// <summary>
        /// Apply updating proccess. All changes will be made. 
        /// </summary>
        /// <returns></returns>
        Model.LocalUserInfo Apply();
    }
}