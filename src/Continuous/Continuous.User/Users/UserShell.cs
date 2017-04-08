using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text.RegularExpressions;
using Continuous.Management.Common;
using Continuous.User.Users.Model;

namespace Continuous.User.Users
{
    public class UserShell : IUserShell
    {
        private readonly ScriptExecutor _executor;
        private readonly ScriptsBoundle _scripts;
        private readonly Regex _userLineRegex = new Regex(@"[\s]{2,}");

        private readonly int nameIndex = 0;
        private readonly int valueIndex = 1;


        public UserShell()
        {
            _executor = new ScriptExecutor(GetType());
            _scripts = new ScriptsBoundle();
        }

        public void Create(UserModel userModel)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", userModel.Name),
                new CommandParameter("password", userModel.Password),
                new CommandParameter("description", userModel.Description),
                new CommandParameter("fullName", userModel.FullName),
                new CommandParameter("expires", userModel.Expires?.ToString("dd/MM/yyyy") ?? "never")
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

            if (!results.Any()) return null;

            return MapToLocalUser(results);
        }

        private UserModel MapToLocalUser(ICollection<PSObject> results)
        {
            var properties = new Dictionary<string, string>();

            foreach (var result in results)
            {
                var propertyLine = _userLineRegex.Split(result.BaseObject.ToString());

                if (HasNameAndValue(propertyLine))
                    properties.Add(propertyLine[nameIndex], propertyLine[valueIndex]);
            }

            return MapToLocalUser(properties);
        }

        private static bool HasNameAndValue(string[] propertyLine)
        {
            return propertyLine.Length == 2;
        }

        private static UserModel MapToLocalUser(Dictionary<string, string> properties)
        {
            var model = new UserModel
            {
                Name = properties["User name"],
                FullName = properties["Full Name"],
                Description = properties["Comment"],
                Password = "",
                Expires = properties["Account expires"] == "Never"
                    ? null
                    : (DateTime?) DateTime.Parse(properties["Account expires"]),
            };

            return model;
        }

        private static void ThrowServiceExceptionIfNecessary(ICollection<PSObject> result, string commandName)
        {
            var returnValue = result.FirstOrDefault()?.BaseObject as string;

            if (returnValue != "The command completed successfully.")
                throw new InvalidOperationException($"Cannot invoke command {commandName}. Reason: " + returnValue);
        }
    }
}