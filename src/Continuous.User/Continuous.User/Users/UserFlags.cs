namespace Continuous.User.Users
{
    internal class UserFlags
    {
        internal const int PasswordNotRequiredFlag = 0x20;
        internal const int PasswordCantChangeFlag = 0x40;
        internal const int PasswordCantExpireFlag = 0x10000;
      
        private readonly int _flags;

        public UserFlags(int flags)
        {
            _flags = flags;
        }

        internal int Flags => _flags;

        internal bool PasswordRequired => !HasFlag(PasswordNotRequiredFlag);

        internal bool PasswordCanBeChangedByUser => !HasFlag(PasswordCantChangeFlag);

        internal bool PasswordCanExpire => !HasFlag(PasswordCantExpireFlag);

        private bool HasFlag(int flag)
        {
            return (_flags & flag) > 0;
        }
    }
}