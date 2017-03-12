using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text.RegularExpressions;
using Continuous.Management.Common;

namespace Continuous.Management.Users
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
            _executor = new ScriptExecutor();
            _scripts = new ScriptsBoundle();
        }

        public void Create(Model.User user)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", user.Name),
                new CommandParameter("password", user.Password),
                new CommandParameter("description", user.Description),
                new CommandParameter("fullName", user.FullName),
                new CommandParameter("expires", user.Expires?.ToString("dd/MM/yyyy") ?? "never")
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

        public Model.User Get(string userName)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", userName)
            };

            var results = _executor.Execute(_scripts.GetUser, parameters);

            if (!results.Any()) return null;

            return MapToLocalUser(results);
        }

        private Model.User MapToLocalUser(ICollection<PSObject> results)
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

        private static Model.User MapToLocalUser(Dictionary<string, string> properties)
        {
            return new Model.User
            {
                Name = properties["User name"],
                FullName = properties["Full Name"],
                Description = properties["Comment"],
                Password = "",
                Expires = DateTime.Parse(properties["Account expires"])
            };
        }

        private static void ThrowServiceExceptionIfNecessary(ICollection<PSObject> result, string commandName)
        {
            var returnValue = result.FirstOrDefault()?.BaseObject as string;

            if (returnValue != "The command completed successfully.")
                throw new InvalidOperationException($"Cannot invoke command {commandName}. Reason: " + returnValue);
        }
    }
}