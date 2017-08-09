using System;
using System.Security;

namespace Continuous.User.Users.Extensions.LocalUserInfo
{
    internal class LocalUserInfoUpdate : ILocalUserInfoUpdate
    {
        private readonly IUserShell _shell;
        private readonly Model.LocalUserInfo _user;

        private readonly ConfigurationCache _cache;
        private ConfigurationCache _backupConfig;

        internal LocalUserInfoUpdate(Model.LocalUserInfo user, IUserShell shell)
        {
            _shell = shell;
            _user = user;

            _cache =  new ConfigurationCache();
        }

        public ILocalUserInfoUpdate Password(SecureString newPassword)
        {
            _cache.Password = newPassword;
            return this;
        }

        public ILocalUserInfoUpdate PasswordRequired(bool newValue)
        {
            _cache.PasswordRequired = newValue;
            return this;
        }

        public ILocalUserInfoUpdate PasswordCanBeChangedByUser(bool newValue)
        {
            _cache.PasswordCanBeChangedByUser = newValue;
            return this;
        }

        public ILocalUserInfoUpdate PasswordCanExpire(bool newValue)
        {
            _cache.PasswordCanExpire = newValue;
            return this;
        }

        public ILocalUserInfoUpdate PasswordExpired(bool newValue)
        {
            _cache.PasswordExpired = newValue;
            return this;
        }

        public ILocalUserInfoUpdate AccountDisabled(bool newValue)
        {
            _cache.AccountDisabled = newValue;
            return this;
        }

        public ILocalUserInfoUpdate IsVisible(bool newValue)
        {
            _cache.IsVisible = newValue;
            return this;
        }

        public ILocalUserInfoUpdate Description(string newValue)
        {
            _cache.Desription = newValue;
            return this;
        }

        public ILocalUserInfoUpdate FullName(string newValue)
        {
            _cache.FullName = newValue;
            return this;
        }

        public ILocalUserInfoUpdate RollbackOnError()
        {
            _cache.RollbackOnError = true;

            if (_cache.RollbackOnError.GetValueOrDefault())
            {
                var service = _shell.GetLocalUser(_user.Name);

                _backupConfig = CreateBackupConfig(service, _cache);
            }

            return this;
        }


        public Model.LocalUserInfo Cancel()
        {
            _cache.Clear();

            return _user;
        }

        public Model.LocalUserInfo Apply()
        {
            try
            {
                Update(_cache);
            }
            catch (Exception)
            {
                if (_cache.RollbackOnError.GetValueOrDefault()) Rollback();

                throw;
            }
            finally
            {
                _cache.Clear();
            }

            _user.Refresh();

            return _user;
        }

        
        private void Update(ConfigurationCache config)
        {
            if(config.AccountDisabled.HasValue)
                _shell.SetAccountDisabled(_user.Name, config.AccountDisabled.Value);

            if (config.Password != null)
                _shell.ChangePassword(_user.Name, config.Password);

            if(config.IsVisible.HasValue)
                _shell.SetUserVisibility(_user.Name, config.IsVisible.Value);

            if (config.PasswordCanBeChangedByUser.HasValue)
                _shell.SetPasswordCanBeChangedByUser(_user.Name, config.PasswordCanBeChangedByUser.Value);

            if (config.PasswordRequired.HasValue)
                _shell.SetPasswordRequired(_user.Name, config.PasswordRequired.Value);

            if (config.PasswordCanExpire.HasValue)
                _shell.SetPasswordCanExpire(_user.Name, config.PasswordCanExpire.Value);

            if (config.PasswordExpired.HasValue)
                _shell.SetPasswordExpired(_user.Name, config.PasswordExpired.Value);

            if(config.Desription != null)
                _shell.SetUserDescription(_user.Name, config.Desription);

            if(config.FullName != null)
                _shell.SetUserFullName(_user.Name, config.FullName);
        }

        private void Rollback()
        {
            Update(_backupConfig);
        }

        private ConfigurationCache CreateBackupConfig(Model.LocalUserInfo actualUser, ConfigurationCache cache)
        {
            var rollback = new ConfigurationCache();

            if (cache.AccountDisabled.HasValue)
                rollback.AccountDisabled = actualUser.AccountDisabled;

            if (cache.IsVisible.HasValue)
                rollback.IsVisible = actualUser.IsVisible;

            if (cache.PasswordCanBeChangedByUser.HasValue)
                rollback.PasswordCanBeChangedByUser = actualUser.PasswordCanBeChangedByUser;

            if (cache.PasswordRequired.HasValue)
                rollback.PasswordRequired = actualUser.PasswordRequired;

            if (cache.PasswordCanExpire.HasValue)
                rollback.PasswordCanExpire = actualUser.PasswordExpires.HasValue;

            if (cache.PasswordExpired.HasValue)
                rollback.PasswordExpired = actualUser.PasswordExpires.HasValue 
                    && actualUser.PasswordExpires.Value.Date > DateTime.Now;

            if (cache.Desription != null)
                rollback.Desription = actualUser.Description;

            return rollback;
        }
    }
}