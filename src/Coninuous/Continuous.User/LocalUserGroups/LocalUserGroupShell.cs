using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Continuous.Management.Common;
using Continuous.Management.Common.Extensions;

namespace Continuous.User.LocalUserGroups
{
    public class LocalUserGroupShell : ILocalUserGroupShell
    {
        private readonly ScriptExecutor _executor;
        private readonly ScriptsBoundle _scripts;
        private readonly Mapper _mapper;

        public LocalUserGroupShell()
        {
            _executor = new ScriptExecutor();
            _scripts = new ScriptsBoundle();
            _mapper = new Mapper();
        }

        public void Create(string name, string description)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", name),       
                new CommandParameter("description", description),
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

            return !results.Any() ? null : _mapper.Map(results);
        }

        public void AssignUsers(string groupName, List<string> userNames)
        {
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
    }
}