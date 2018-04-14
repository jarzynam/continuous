using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation.Runspaces;
using System.Security;
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
                new CommandParameter("fullName", userModel.FullName)
            };

            _executor.Execute(_scripts.CreateUser, parameters);

            SetAccountExpirationDate(userModel.Name, userModel.AccountExpires);
        }

        public void Remove(string userName)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", userName)
            };

            _executor.Execute(_scripts.RemoveUser, parameters);
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

            var user =  _userMapper.MapToLocalUserInfo(results.FirstOrDefault());
            
            user.IsVisible = IsUserVisible(userName);

            return user;
        }

        public void ChangePassword(string userName, string userPassword)
        {
            ThrowIfNotExist(userName);

            var parameters = new List<CommandParameter>
            {
                new CommandParameter("userName", userName),
                new CommandParameter("password", userPassword)
            };

            _executor.Execute(_scripts.ChangeUserPassword, parameters);
        }

        public void ChangePassword(string userName, SecureString userPassword)
        {
            ThrowIfNotExist(userName);

            var parameters = new List<CommandParameter>
            {
                new CommandParameter("userName", userName),
                new CommandParameter("password", userPassword)
            };

            _executor.Execute(_scripts.ChangeUserPassword, parameters);
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

        public void SetUserVisibility(string userName, bool value)
        {
            ThrowIfNotExist(userName);

            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", userName),
                new CommandParameter("isVisible", value)
            };

            _executor.Execute(_scripts.SetUserVisibility, parameters);
        }

        public void SetUserDescription(string userName, string description)
        {
            SetUserProperty(userName, "Description", description);
        }

        public void SetUserFullName(string userName, string fullName)
        {
            SetUserProperty(userName, "FullName", fullName);
        }

        public void SetAccountExpirationDate(string userName, DateTime? expirationTime = null)
        {
            if (!expirationTime.HasValue)
            {
                SetUserFlags(userName, UserFlags.PasswordCantExpireFlag, true);
            }
            else
            {
                SetUserFlags(userName, UserFlags.PasswordCantExpireFlag, false);
                SetUserPropertyDate(userName, "AccountExpirationDate", expirationTime.Value);
            }
        }


        public LocalUserInfo GetLoggedInUser()
        {
            var userWithDomain = _executor.Execute(_scripts.GetLoggedUsername, new List<CommandParameter>(0))
                .FirstOrDefault();

            var userName = GetUserName(userWithDomain?.BaseObject as string);

            return GetLocalUser(userName);
        }

        public List<LocalUserInfo> GetAllUsers()
        {
            var userObjects = _executor.Execute(_scripts.GetAllUsers, new List<CommandParameter>(0));

            return userObjects.Select(p => _userMapper.MapToLocalUserInfo(p)).ToList();
        }

        private string GetUserName(string username)
        {
            var separatorIndex = username.IndexOf('\\');

            if (separatorIndex == -1) return username;

            return username.Substring(separatorIndex + 1);
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

        private void SetUserPropertyDate(string userName, string propertyName, DateTime value)
        {
            ThrowIfNotExist(userName);

            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", userName),
                new CommandParameter("propertyName", propertyName),
                new CommandParameter("propertyValue", value)
            };

            _executor.Execute(_scripts.SetUserPropertyDate, parameters);
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

        private bool IsUserVisible(string userName)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", userName)
            };

            var results = _executor.Execute(_scripts.GetUserVisibility, parameters);

            if (results.Any())
            {
                return (bool) results.First().BaseObject;
            }

            return false;
        }

        private void ThrowIfNotExist(string userName)
        {
            if (!Exists(userName))
                throw new InvalidOperationException($"User '{userName}' is not existing");
        }
    }
}