
using System.Security;

namespace Continuous.User.Users.Extensions.LocalUserInfo
{
    internal class ConfigurationCache
    {
        internal bool? RollbackOnError { get; set; }

        public SecureString Password { get; set; }
        public bool? PasswordCanExpire { get; set; }
        public bool? PasswordExpired { get; set; }
        public bool? PasswordRequired { get; set; }
        public bool? PasswordCanBeChangedByUser { get; set; }
        public bool? AccountDisabled { get; set; }
        public bool? IsVisible { get; set; }
        public string Desription { get; set; }

        internal void Clear()
        {
            RollbackOnError = false;
        }
    }
}