using System;
using System.Linq;
using Continuous.User.LocalUserGroups.Model;
using System.Collections.Generic;

namespace Continuous.User.Tests.TestHelpers
{
    public static class LocalGroupHelper
    {
        public static void Create(string groupName)
        {
            ScriptInvoker.InvokeScript($"net localgroup {groupName} /add /comment:\"test group to delete\" ");
        }

        public static void Remove(string groupName)
        {
            ScriptInvoker.InvokeScript($"net localgroup {groupName} /delete");
        }

        public static LocalUserGroup GetGroup(string groupName)
        {
            var result = ScriptInvoker.InvokeScript($"Get-WMIObject -Class Win32_Group -Filter \"Name = '{groupName}'\"").FirstOrDefault();

            if (result == null) return null;

            var model = new LocalUserGroup
            {
                Name = (string) result.Properties["Name"].Value,
                Description = (string) result.Properties["Description"].Value,
                Members = GetMemebers(groupName)
            };

            return model;
        }

        public static List<string> GetMemebers(string groupName)
        {
            var script = @"net localgroup "+groupName +" | where { $_ } | select -skip 4";
            var result = ScriptInvoker.InvokeScript(script);

            return result.Select(p => p.BaseObject as string)
                .Take(result.Count-1).ToList();
        }

        public static void AssignUser(string groupName, string userName)
        {
            var script = $"net localgroup {groupName} {userName} /add";

            ScriptInvoker.InvokeScript(script);
        }

        public static void RemoveUser(string groupName, string userName)
        {
            var script = $"net localgroup {groupName} {userName} /delete";

            ScriptInvoker.InvokeScript(script);
        }

        public static string GetSid(string groupName)
        {
            return GetProperty(groupName, "SID") as string;
        }

        private static object GetProperty(string groupName, string propertyName)
        {
            var script =
                $"(Get-WMIObject -class Win32_Group -Filter \"Name= '{groupName}' and LocalAccount= '$true'\").{propertyName}";

            return ScriptInvoker.InvokeScript(script).FirstOrDefault()?.BaseObject;
        }



    }
}
