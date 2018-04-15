using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Continuous.Management.Common;
using Continuous.User.LocalUserGroups.Model;

namespace Continuous.User.LocalUserGroups
{
    public class LocalUserGroupShell : ILocalUserGroupShell
    {
        private readonly ScriptExecutor _executor;
        private readonly LocalUserGroupMapper _localUserGroupMapper;
        private readonly ScriptsBoundle _scripts;

        public LocalUserGroupShell()
        {
            _executor = new ScriptExecutor(GetType());
            _scripts = new ScriptsBoundle();
            _localUserGroupMapper = new LocalUserGroupMapper();
        }

        public void Create(string name, string description)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", name),
                new CommandParameter("description", description)
            };

            _executor.Execute(_scripts.CreateLocalUserGroup, parameters);
        }

        public void Remove(string groupName)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", groupName)
            };

            _executor.Execute(_scripts.RemoveLocalUserGroup, parameters);
        }

        public LocalUserGroup Get(string groupName)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", groupName)
            };
         
            var results = _executor.Execute(_scripts.GetLocalUserGroup, parameters);

            var group =  _localUserGroupMapper.Map(results.FirstOrDefault());

            if(group != null) group.Members = GetMembers(groupName);

            return group;
        }

        public List<string> GetMembers(string groupName)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", groupName)
            };
           
            var results = _executor.Execute(_scripts.GetLocalUserGroupMembers, parameters);

            return results.Select(p => p.BaseObject as string).ToList();

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

            foreach (var userName in userNames) AssignUser(groupName, userName);
        }

        public void RemoveUsers(string groupName, List<string> userNames)
        {
            ThrowIfListIsEmpty(userNames);

            foreach (var userName in userNames) RemoveUser(groupName, userName);
        }

        public void AssignUser(string groupName, string userName)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", groupName),
                new CommandParameter("userName", userName)
            };

            try
            {
                _executor.Execute(_scripts.AddUsersToLocalGroup, parameters);
            }
            catch (ExtendedTypeSystemException ex)
            {
                throw new MethodInvocationException(ex.Message, ex);
            }
        }

        public void RemoveUser(string groupName, string userName)
        {
            var parameters = new List<CommandParameter>
            {
                new CommandParameter("name", groupName),
                new CommandParameter("userName", userName)
            };

            try
            {
                _executor.Execute(_scripts.RemoveUsersFromLocalGroup, parameters);
            }
            catch (ExtendedTypeSystemException ex)
            {
                throw new MethodInvocationException(ex.Message, ex);
            }
        }

        private static void ThrowIfListIsEmpty(List<string> userNames)
        {
            if (userNames == null || !userNames.Any())
                throw new MethodInvocationException($"the {nameof(userNames)} list cannot be empty");
        }
    }
}