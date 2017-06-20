using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Continuous.Management.Common;
using Continuous.Management.Common.Extensions;
using Continuous.User.Users.Model;

namespace Continuous.User.Users
{
    public class UserShell : IUserShell
    {
        private readonly ScriptExecutor _executor;
        private readonly ScriptsBoundle _scripts;
        private readonly UserMapper _userMapper;

        public UserShell()
        {
            _executor = new ScriptExecutor(GetType());
            _scripts = new ScriptsBoundle();
            _userMapper = new UserMapper();
        }

        [Obsolete("Use create with LocalUserCreateModel as input parameter")]
        public void Create(UserModel userModel)
        {
           Create(_userMapper.MapToUserCreateModel(userModel));
        }

        public void Create(LocalUserCreateModel userModel)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", userModel.Name),
                new CommandParameter("password", userModel.Password),
                new CommandParameter("description", userModel.Description),
                new CommandParameter("fullName", userModel.FullName),
                new CommandParameter("expires", userModel.AccountExpires?.Date.ToShortDateString() ?? "never")
            };

            var result = _executor.Execute(_scripts.CreateUser, parameters);

            ThrowServiceExceptionIfNecessary(result, nameof(Create));
        }

        public void Remove(string userName)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", userName)
            };

            var result = _executor.Execute(_scripts.RemoveUser, parameters);

            ThrowServiceExceptionIfNecessary(result, nameof(Remove));
        }

        [Obsolete("Use GetLocalUser")]
        public UserModel Get(string userName)
        {
            var results = GetLocalUser(userName);

            return _userMapper.MapToUserModel(results);
        }

        public LocalUserInfo GetLocalUser(string userName)
        {
            if (!Exists(userName)) return null;

            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", userName)
            };

            var results = _executor.Execute(_scripts.GetUser, parameters);

            return _userMapper.MapToLocalUserInfo(results);
        }

        public void ChangePassword(string userName, string userPassword)
        {
            ThrowIfNotExist(userName);

            var parameters = new List<CommandParameter>
            {
                new CommandParameter("userName", userName),
                new CommandParameter("password", userPassword)
            };

            var result = _executor.Execute(_scripts.ChangeUserPassword, parameters);

            ThrowServiceExceptionIfNecessary(result, nameof(ChangePassword));
        }

        public bool Exists(string userName)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("userName", userName)
            };

            var result = _executor.Execute(_scripts.ExistsUser, parameters).FirstOrDefault();

            return result?.BaseObject is bool && (bool) result.BaseObject;
        }

        public void SetPasswordRequired(string userName, bool value)
        {
            SetUserFlags(userName, UserFlags.PasswordNotRequiredFlag, !value);
        }

        public void SetPasswordCanBeChangedByUser(string userName, bool value)
        {
            SetUserFlags(userName, UserFlags.PasswordCantChangeFlag, !value);
        }

        public void SetPasswordCanExpire(string userName, bool value)
        {
            SetUserFlags(userName, UserFlags.PasswordCantExpireFlag, !value);
        }

        public void SetPasswordExpired(string userName, bool value)
        {
            SetUserProperty(userName, "PasswordExpired", value.ToInteger());
        }

        public void SetAccountDisabled(string userName, bool value)
        {
            SetUserFlags(userName, UserFlags.AccountDisabledFlag, value);
        }

        private void SetUserProperty(string userName, string propertyName, object value)
        {
            ThrowIfNotExist(userName);

            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", userName),
                new CommandParameter("propertyName", propertyName),
                new CommandParameter("propertyValue", value)
            };

            _executor.Execute(_scripts.SetUserProperty, parameters);
        }

        private void SetUserFlags(string userName, int flag, bool value)
        {
            ThrowIfNotExist(userName);

            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", userName),
                new CommandParameter("flag", flag),
                new CommandParameter("value", value)
            };

             _executor.Execute(_scripts.SetUserFlag, parameters);
        }

        private static void ThrowServiceExceptionIfNecessary(ICollection<PSObject> result, string commandName)
        {
            var returnValue = result.FirstOrDefault()?.BaseObject as string;

            if (returnValue != "The command completed successfully.")
                throw new InvalidOperationException($"Cannot invoke command {commandName}. Reason: " + returnValue);
        }

        private void ThrowIfNotExist(string userName)
        {
            if(!Exists(userName))
                throw new InvalidOperationException($"User '{userName}' is not existing");
        }
        
    }
}