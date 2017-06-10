using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Continuous.User.Users.Model;

namespace Continuous.User.Users
{
    internal class UserMapper
    {
        internal UserModel MapToLocalUser(ICollection<PSObject> results)
        {
            var result = results.FirstOrDefault();

            if (result == null) return null;

            var properties = new UserResultProperties(result);

            var flags = properties.Get("UserFlags") ?? default(int);
            
            var userFlags = new UserFlags((int) flags);

            var user = new UserModel();

            user.Name = properties.Get("Name") as string;
            user.FullName = properties.Get("FullName") as string;
            user.Description = properties.Get("Description") as string;
            user.PasswordCanChange = userFlags.PasswordCanBeChangedByUser;
            user.PasswordRequired = userFlags.PasswordRequired;

            user.PasswordMaxBadAttempts = (int?)  properties.Get("MaxBadPasswordsAllowed")?? default(int);

            user.PasswordBadAttemptsInterval = TimeSpan.FromSeconds((int?) properties.Get("LockoutObservationInterval") ?? default(int));
            user.PasswordMinLength = (int?) properties.Get("MinPasswordLength")?? default(int);

            var accountExpirationDate = properties.Get("AccountExpirationDate");
            var passwordAge = properties.Get("PasswordAge");
            var maxPasswordAge =  properties.Get("MaxPasswordAge");
            var passwordExpired = properties.Get("PasswordExpired");

            user.PasswordLastChange = passwordAge == null?  null : (DateTime?) DateTime.Now.AddSeconds(-(int) passwordAge);

            user.AccountExpires = accountExpirationDate == null
                ? null
                : GetExpirationDate(0, (int?) accountExpirationDate);

            user.PasswordExpires = userFlags.PasswordCanExpire
                ? GetExpirationDate( (int?) passwordAge, (int?) maxPasswordAge)
                : null;
        
            user.PasswordMustChangeOnNextLogon = IsPasswordChangeRequiredOnNextLogon((int) passwordAge, (int) passwordExpired);

            return user;
        }

        private bool IsPasswordChangeRequiredOnNextLogon(long passwordAge, int passwordExpired)
        {
            return passwordAge == 0 && passwordExpired == 1;
        }
        private DateTime? GetExpirationDate(long? age, long? maxAge)
        {
            var deltaTime = TimeSpan.FromSeconds(maxAge.GetValueOrDefault()) -
                TimeSpan.FromSeconds(age.GetValueOrDefault());
            
            return DateTime.Now.AddTicks(deltaTime.Ticks);
        }
    }
}