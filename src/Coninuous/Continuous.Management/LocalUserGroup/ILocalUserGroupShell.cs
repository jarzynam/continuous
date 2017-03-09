using System.Collections.Generic;

namespace Continuous.Management.LocalUserGroup
{
    public interface ILocalUserGroupShell
    {
        void Create(string name, string description);
        void Remove(string groupName);
        Model.LocalUserGroup Get(string groupName);
        void AssignUsers(string groupName, List<string> userNames);
        void RemoveUsers(string groupName, List<string> userNames);
    }
}