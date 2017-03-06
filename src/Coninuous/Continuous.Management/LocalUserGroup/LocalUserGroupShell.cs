using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Text.RegularExpressions;
using Continuous.Management.Common;

namespace Continuous.Management.LocalUserGroup
{
    public class LocalUserGroupShell
    {
        private readonly ScriptExecutor _executor;
        private readonly ScriptsBoundle _scripts;
        private readonly Regex _wihteSpaceSeparatorRegex = new Regex(@"[\s]{2,}");

        private readonly int nameIndex = 0;
        private readonly int valueIndex = 1;


        public LocalUserGroupShell()
        {
            _executor = new ScriptExecutor();
            _scripts = new ScriptsBoundle();
        }

        public void Create(Model.LocalUserGroup localGroup)
        {

            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", localGroup.Name),
                
                new CommandParameter("description", localGroup.Description),
                
                new CommandParameter("members", FlattenCollectionToString(localGroup.Members))
            };

            var result = _executor.Execute(_scripts.CreateLocalUserGroup, parameters);

            ThrowServiceExceptionIfNecessary(result, nameof(Create));
        }

        public void Remove(string groupName)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", groupName)
            };

            var result = _executor.Execute(_scripts.RemoveLocalUserGroup, parameters);

            ThrowServiceExceptionIfNecessary(result, nameof(Remove));
        }

        public Model.LocalUserGroup Get(string groupName)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", groupName)
            };

            var results = _executor.Execute(_scripts.GetLocalUserGroup, parameters);

            if (!results.Any()) return null;

            return Map(results);
        }

        public void AddUserToGroup(string groupName, string userName, string userDomain = null)
        {
            userName = SplitDomainWithUserName(userName, userDomain);

            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", groupName),
                new CommandParameter("userName", userName)
            };

            var results = _executor.Execute(_scripts.AddUserToLocalGroup, parameters);

            ThrowServiceExceptionIfNecessary(results, nameof(AddUserToGroup));
        }

        public void RemoveUserFromGroup(string groupName, string userName, string userDomain = null)
        {
            userName = SplitDomainWithUserName(userName, userDomain);

            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", groupName),
                new CommandParameter("userName", userName)
            };

            var result = _executor.Execute(_scripts.RemoveUserFromLocalGroup, parameters);

            ThrowServiceExceptionIfNecessary(result, nameof(RemoveUserFromGroup));

        }

        private Model.LocalUserGroup Map(ICollection<PSObject> results)
        {
            var properties = new Dictionary<string, string>();

            foreach (var result in results)
            {
                var propertyLine = _wihteSpaceSeparatorRegex.Split(result.BaseObject.ToString());

                properties.Add(propertyLine[nameIndex], propertyLine[valueIndex]);
            }

            return new Model.LocalUserGroup
            {
                Name = properties["Alias name"],
                Description = properties["Comment"],
                Members = properties["Members"]
                    .Replace("-", "")
                    .Replace("The command completed successfully.", "")
                    .Split('\n')
                    .Where(p => !String.IsNullOrEmpty(p))
                    .ToList()
            };
        }

        private static void ThrowServiceExceptionIfNecessary(ICollection<PSObject> result, string commandName)
        {
            var returnValue = result.FirstOrDefault()?.BaseObject as string;

            if (returnValue != "The command completed successfully.")
                throw new InvalidOperationException($"Cannot invoke command {commandName}. Reason: " + returnValue);
        }

        private static string FlattenCollectionToString(List<string> collection)
        {
            var builder = new StringBuilder();

            for (var i = 0; i < collection.Count; i++)
            {
                builder.Append(collection[i]);

                if (i < collection.Count - 1)
                    builder.Append(" ");
            }

            return builder.ToString();
        }

        private static string SplitDomainWithUserName(string userName, string userDomain)
        {
            if (userDomain != null)
                userName = userDomain + @"\" + userName;
            return userName;
        }
    }
}