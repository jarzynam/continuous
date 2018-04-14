using System.Collections.Generic;
using System.Management.Automation;
using Continuous.User.Shared;

namespace Continuous.User.LocalUserGroups
{
    internal class LocalUserGroupMapper
    {

        internal Model.LocalUserGroup Map(PSObject result)
        {
            var properties = new PsObjectProperties(result);

            var model = new Model.LocalUserGroup();

            model.Name = properties.Get<string>("Name");
            model.Description = properties.Get<string>("Description");

            model.Members = new List<string>();

            return model.Name == null? null : model;

        }
        
    }
}