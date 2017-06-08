using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Management.Automation;

namespace Continuous.User.Users
{
    internal class UserResultProperties
    {
        private readonly List<PropertyValueCollection> _properties;

        internal UserResultProperties(PSObject result)
        {
            _properties = result.Properties
                .Where(p => p?.TypeNameOfValue == "System.DirectoryServices.PropertyValueCollection")
                .Select(p => (PropertyValueCollection) p.Value)
                .ToList();
        }

        internal object Get(string propertyName)
        {
            return _properties.FirstOrDefault(p => p.PropertyName == propertyName)
                ?.Value;
        }
    }
}