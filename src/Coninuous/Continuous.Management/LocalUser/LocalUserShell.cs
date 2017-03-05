using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text.RegularExpressions;
using Continuous.Management.Common;

namespace Continuous.Management.LocalUser
{
    public class LocalUserShell
    {
        private readonly ScriptExecutor _executor;
        private readonly ScriptsBoundle _scripts;

        public LocalUserShell()
        {
            _executor = new ScriptExecutor();
            _scripts = new ScriptsBoundle();
        }

        public void CreateUser(Model.LocalUser user)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", user.Name),
                new CommandParameter("password", user.Password),
                new CommandParameter("description", user.Description),
                new CommandParameter("fullName", user.FullName),
                new CommandParameter("expires", user.Expires?.ToString("dd/MM/yyyy") ?? "never"),
            };

            var result = _executor.Execute(_scripts.CreateUser, parameters);

            ThrowServiceExceptionIfNecessary(result, nameof(CreateUser));
        }

        public void RemoveUser(string userName)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", userName)
            };

            var result = _executor.Execute(_scripts.RemoveUser, parameters);

            ThrowServiceExceptionIfNecessary(result, nameof(RemoveUser));
        }

        private readonly Regex _regex = new Regex(@"[\s]{2,}");

        public Model.LocalUser GetUser(string userName)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", userName)
            };

            var results = _executor.Execute(_scripts.GetUser, parameters);

            if (results.Count > 0)
            {
                var dict = new Dictionary<string, string>();
                foreach (var result in results)
                {
                   var m =  _regex.Split(result.BaseObject.ToString());

                    if(m.Length == 2)
                        dict.Add(m[0], m[1]);
                }

                return new Model.LocalUser
                {
                    Name = dict["User name"],
                    FullName = dict["Full Name"],
                    Description = dict["Comment"],
                    Password = "",
                    Expires = DateTime.Parse(dict["Account expires"])
                };
            }


            return null;
        }

       private static void ThrowServiceExceptionIfNecessary(ICollection<PSObject> result, string commandName)
        {
            var returnValue = (result.FirstOrDefault()?.BaseObject as string);

            if (returnValue != "The command completed successfully.")
                throw new InvalidOperationException($"Cannot invoke command {commandName}. Reason: " + returnValue);
        }

    }
}
