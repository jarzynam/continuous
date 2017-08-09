using System;
using Continuous.User.Users.Extensions;

namespace Continuous.User.Users.Model
{
    /// <summary>
    ///     Model for creating new local user
    /// </summary>
    public class LocalUserInfo : LocalUserInfoExtensions
    {
        public LocalUserInfo()
        {
            InitializeBase(this);
        }

        public LocalUserInfo(string userName) : this()
        {
            Name = userName;
            Refresh();
        }

        /// <summary>
        ///     User security id 
        /// </summary>
        public string Sid { get; set; }

        /// <summary>
        ///     User name
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        ///     User full name
        /// </summary>
        public string FullName { get; internal set; }

        /// <summary>
        ///     User description
        /// </summary>
        public string Description { get; internal set; }

        /// <summary>
        ///     Determines wheter account is disabled.
        /// </summary>
        public bool AccountDisabled { get; internal set; }

        /// <summary>
        ///     Determines wheter account is locked out
        /// </summary>
        public bool AccountLocked { get; internal set; }

        /// <summary>
        ///     Determines when account will be unlock after locked by user
        /// </summary>
        public TimeSpan AutoUnlockInterval { get; internal set; }

        /// <summary>
        ///     User account expiration date - if null, the account will never exipre
        /// </summary>
        public DateTime? AccountExpires { get; internal set; }

        /// <summary>
        ///     User password expiration date - if null, password will never expire
        /// </summary>
        public DateTime? PasswordExpires { get; internal set; }

        /// <summary>
        ///     Last date when password has been changed
        /// </summary>
        public DateTime? PasswordLastChange { get; internal set; }

        /// <summary>
        ///     Last date when user logon to account
        /// </summary>
        public DateTime? LastLogon { get; internal set; }

        /// <summary>
        ///     Determines if user must change password at next logon
        /// </summary>
        public bool PasswordMustBeChangedAtNextLogon { get; internal set; }

        /// <summary>
        ///     Minimum length that password must have
        /// </summary>
        public long PasswordMinLength { get; internal set; }

        /// <summary>
        ///     How many times user can type wrong password
        /// </summary>
        public int PasswordMaxBadAttempts { get; internal set; }

        /// <summary>
        ///     For how long system will be remember number of bad password attempts
        /// </summary>
        public TimeSpan PasswordBadAttemptsInterval { get; internal set; }

        /// <summary>
        ///     User can change password by himself
        /// </summary>
        public bool PasswordCanBeChangedByUser { get; internal set; }

        /// <summary>
        ///     Password is required to logon
        /// </summary>
        public bool PasswordRequired { get; internal set; }

        /// <summary>
        ///     User visibility in windows welcome screen
        /// </summary>
        public bool IsVisible { get; internal set; }
    }
}