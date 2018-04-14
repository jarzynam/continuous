using System;
using System.Management.Automation;
using Continuous.User.Users.Model;
#pragma warning disable 612

namespace Continuous.User.Users
{
    internal class UserMapper
    {


        internal LocalUserCreateModel MapToUserCreateModel(UserModel model)
        {
            return new LocalUserCreateModel
            {
                Name = model.Name,
                Description = model.Description,
                Password = model.Password,
                FullName = model.FullName,
                AccountExpires = model.AccountExpires
            };
        }

        internal UserModel MapToUserModel(LocalUserInfo info)
        {
            if (info == null) return null;

            return new UserModel
            {
                Name = info.Name,
                Description = info.Description,
                Password = null,
                FullName = info.FullName,
                PasswordMaxBadAttempts = info.PasswordMaxBadAttempts,
                PasswordRequired = info.PasswordRequired,
                PasswordExpires = info.PasswordExpires,
                PasswordMinLength = info.PasswordMinLength,
                AccountExpires = info.AccountExpires,
                PasswordBadAttemptsInterval = info.PasswordBadAttemptsInterval,
                PasswordCanBeChangedByUser = info.PasswordCanBeChangedByUser,
                PasswordLastChange = info.PasswordLastChange,
                PasswordMustBeChangedAtNextLogon = info.PasswordMustBeChangedAtNextLogon
            };
        }

        internal LocalUserInfo MapToLocalUserInfo(PSObject result)
        {
            if (result == null) return null;

            var properties = new UserResultProperties(result);

            var flags = properties.Get("UserFlags") ?? default(int);

            var userFlags = new UserFlags((int)flags);

            var user = new LocalUserInfo();

            user.Sid = result.Properties["SecurityId"]?.Value as string;
            user.Name = properties.Get<string>("Name");
            user.FullName = properties.Get<string>("FullName");
            user.Description = properties.Get<string>("Description");

            user.PasswordCanBeChangedByUser = userFlags.PasswordCanBeChangedByUser;
            user.PasswordRequired = userFlags.PasswordRequired;
            user.AccountDisabled = userFlags.AccountDisabled;
            user.AccountLocked = userFlags.AccountLocked;

            user.PasswordMaxBadAttempts = properties.GetValue<int>("MaxBadPasswordsAllowed");

            user.PasswordBadAttemptsInterval = properties.GetTimeSpan("LockoutObservationInterval");
            user.AutoUnlockInterval = properties.GetTimeSpan("AutoUnlockInterval");
            user.LastLogon = properties.GetDateTime("LastLogin");
            user.PasswordMinLength = properties.GetValue<int>("MinPasswordLength");

            var accountExpirationDate = properties.GetTimeSpan("AccountExpirationDate");
            var passwordAge = properties.GetTimeSpan("PasswordAge");
            var maxPasswordAge = properties.GetTimeSpan("MaxPasswordAge");
            var passwordExpired = properties.GetValue<int>("PasswordExpired");

            user.PasswordLastChange = DateTime.Now.Add(passwordAge.Negate()).Date;

            user.AccountExpires = GetExpirationDate(TimeSpan.Zero, accountExpirationDate);

            user.PasswordExpires = userFlags.PasswordCanExpire
                ? GetExpirationDate(passwordAge, maxPasswordAge)
                : null;

            user.PasswordMustBeChangedAtNextLogon = IsPasswordChangeRequiredOnNextLogon(passwordAge, passwordExpired);

            return user;
        }

        private bool IsPasswordChangeRequiredOnNextLogon(TimeSpan passwordAge, int passwordExpired)
        {
            return passwordAge == TimeSpan.Zero && passwordExpired == 1;
        }
        private DateTime? GetExpirationDate(TimeSpan age, TimeSpan maxAge)
        {
            var deltaTime = maxAge - age;

            if (deltaTime == TimeSpan.Zero) return null;

            return DateTime.Now.Add(deltaTime).Date;
        }

        public void CopyProperties(LocalUserInfo info, LocalUserInfo source)
        {
            info.IsVisible = source.IsVisible;
            info.AccountDisabled = source.AccountDisabled;
            info.AccountExpires = source.AccountExpires;
            info.AccountLocked = source.AccountLocked;
            info.AutoUnlockInterval = source.AutoUnlockInterval;
            info.Description = source.Description;
            info.FullName = source.FullName;
            info.LastLogon = source.LastLogon;
            info.Name = source.Name;
            info.PasswordBadAttemptsInterval = source.PasswordBadAttemptsInterval;
            info.PasswordCanBeChangedByUser = source.PasswordCanBeChangedByUser;
            info.PasswordExpires = source.PasswordExpires;
            info.PasswordBadAttemptsInterval = source.PasswordBadAttemptsInterval;
            info.PasswordMaxBadAttempts = source.PasswordMaxBadAttempts;
            info.PasswordMinLength = source.PasswordMinLength;
            info.PasswordMustBeChangedAtNextLogon = source.PasswordMustBeChangedAtNextLogon;
            info.PasswordRequired = source.PasswordRequired;
            info.PasswordLastChange = source.PasswordLastChange;
        }
    }
}