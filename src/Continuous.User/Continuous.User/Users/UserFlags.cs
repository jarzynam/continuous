namespace Continuous.User.Users
{
    internal class UserFlags
    {
        private readonly int _flags;

        public UserFlags(int flags)
        {
            _flags = flags;
        }

        internal bool PasswordRequired => !HasFlag(_flags, 0x20);
        internal bool PasswordCanChange => !HasFlag(_flags, 0x40);
        internal bool DontExpirePassword => HasFlag(_flags, 0x1000);


        private bool HasFlag (long flags, int flag)
        {
            return (flags & flag) > 0;
        }
    }
}