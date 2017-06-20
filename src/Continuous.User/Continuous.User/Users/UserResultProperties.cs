using System;
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

        internal TimeSpan GetTimeSpan(string propertyName)
        {
            return TimeSpan.FromSeconds((int?) Get(propertyName) ?? default(int));
        }

        internal T Get<T>(string propertyName) where T: class
        {
            return Get(propertyName) as T;
        }

        internal T GetValue<T>(string propertyName) where T: struct
        {
            return  (T?) Get(propertyName) ?? default(T);
        }

        public DateTime? GetDateTime(string propertyName)
        {
            var property = Get(propertyName);

            return (DateTime?) property;
        }
    }
}