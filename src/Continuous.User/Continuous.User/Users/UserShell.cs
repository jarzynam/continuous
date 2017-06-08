using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Continuous.Management.Common;
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

        public void Create(UserModel userModel)
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

        public UserModel Get(string userName)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", userName)
            };

            var results = _executor.Execute(_scripts.GetUser, parameters);

            return _userMapper.MapToLocalUser(results);
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

        private static void ThrowServiceExceptionIfNecessary(ICollection<PSObject> result, string commandName)
        {
            var returnValue = result.FirstOrDefault()?.BaseObject as string;

            if (returnValue != "The command completed successfully.")
                throw new InvalidOperationException($"Cannot invoke command {commandName}. Reason: " + returnValue);
        }

        private void ThrowIfNotExist(string userName)
        {
            if(Get(userName) == null )
                throw new InvalidOperationException($"User '{userName}' is not existing");
        }
        
    }
}