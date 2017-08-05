using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Continuous.Management.Common;
using Continuous.Management.Common.Extensions;
using Continuous.User.LocalUserGroups.Model;

namespace Continuous.User.LocalUserGroups
{
    public class LocalUserGroupShell : ILocalUserGroupShell
    {
        private readonly ScriptExecutor _executor;
        private readonly Mapper _mapper;
        private readonly ScriptsBoundle _scripts;

        public LocalUserGroupShell()
        {
            _executor = new ScriptExecutor(GetType());
            _scripts = new ScriptsBoundle();
            _mapper = new Mapper();
        }

        public void Create(string name, string description)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", name),
                new CommandParameter("description", description)
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

        public LocalUserGroup Get(string groupName)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", groupName)
            };
            try
            {
                var results = _executor.Execute(_scripts.GetLocalUserGroup, parameters);

                return !results.Any() ? null : _mapper.Map(results);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public LocalUserGroup GetBySid(string sid)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("sid", sid)
            };

            var result = _executor.Execute(_scripts.GetLocalUserGroupBySid, parameters).FirstOrDefault();

            var groupName = result?.Properties["Name"]?.Value as string;

            return groupName != null ? Get(groupName) : null;
        }

        public void AssignUsers(string groupName, List<string> userNames)
        {
            ThrowIfListIsEmpty(userNames);

            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", groupName),
                new CommandParameter("members", userNames.ToFlatString())
            };

            var results = _executor.Execute(_scripts.AddUsersToLocalGroup, parameters);

            ThrowServiceExceptionIfNecessary(results, nameof(AssignUsers));
        }

        public void RemoveUsers(string groupName, List<string> userNames)
        {
            ThrowIfListIsEmpty(userNames);

            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", groupName),
                new CommandParameter("members", userNames.ToFlatString())
            };

            var result = _executor.Execute(_scripts.RemoveUsersFromLocalGroup, parameters);

            ThrowServiceExceptionIfNecessary(result, nameof(RemoveUsers));
        }

        private static void ThrowServiceExceptionIfNecessary(ICollection<PSObject> result, string commandName)
        {
            var returnValue = result.FirstOrDefault()?.BaseObject as string;

            if (returnValue != "The command completed successfully.")
                throw new InvalidOperationException($"Cannot invoke command {commandName}. Reason: " + returnValue);
        }

        private static void ThrowIfListIsEmpty(List<string> userNames)
        {
            if (userNames == null || !userNames.Any())
                throw new InvalidOperationException("the user names list cannot be empty");
        }
    }
}