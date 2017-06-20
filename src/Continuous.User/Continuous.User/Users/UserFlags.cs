namespace Continuous.User.Users
{
    internal class UserFlags
    {
        internal const int PasswordNotRequiredFlag = 0x20;
        internal const int PasswordCantChangeFlag = 0x40;
        internal const int PasswordCantExpireFlag = 0x10000;
        internal const int AccountDisabledFlag = 0x2;
        internal const int AccountLockedOutFlag = 0x10;

        private readonly int _flags;

        public UserFlags(int flags)
        {
            _flags = flags;
        }

        internal int Flags => _flags;

        internal bool PasswordRequired => !HasFlag(PasswordNotRequiredFlag);

        internal bool PasswordCanBeChangedByUser => !HasFlag(PasswordCantChangeFlag);

        internal bool PasswordCanExpire => !HasFlag(PasswordCantExpireFlag);

        internal bool AccountDisabled => HasFlag(AccountDisabledFlag);

        internal bool AccountLocked => HasFlag(AccountLockedOutFlag);

        private bool HasFlag(int flag)
        {
            return (_flags & flag) > 0;
        }
    }
}