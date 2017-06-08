using System;
using System.Linq;
using Continuous.User.Users.Model;

namespace Continuous.User.Tests.TestHelpers
{
    public static class UserHelper
    {

        public static void CreateUser(string userName, string userPassword)
        {
            ScriptInvoker.InvokeScript($"net user {userName} {userPassword} /add");
        }

        public static void DeleteUser(string userName)
        {
            ScriptInvoker.InvokeScript($"net user {userName} /delete");
        }

        public static UserModel GetUser(string userName)
        {
            var result = ScriptInvoker.InvokeScript($"Get-WMIObject -Class Win32_UserAccount -Filter \"Name = '{userName}'\"").FirstOrDefault();

            if (result == null) return null;

            var model =  new UserModel
            {
                Name = (string) result.Properties["Name"].Value,
                Description = (string) result.Properties["Description"].Value,
                FullName = (string) result.Properties["FullName"].Value,
                Password = null
            };
            
            var p = GetUserProperty(userName, "Account expires");
            model.AccountExpires = p == "Never" ? null : (DateTime?) DateTime.Parse(p);
            

            return model;
        }

        public static UserModel BuildLocalUser(string name)
        {
            return new UserModel
            {
                Name = name,
                FullName = "Test User 1",
                Description = "Delete this user after tests",
                Password = "Test123",
                AccountExpires = new DateTime(2018, 01, 01)
            };
        }

        private static string GetUserProperty(string userName, string propertyName)
        {
            var regex = @"'\s{2,}'";
            var result = ScriptInvoker.InvokeScript($"(((net user {userName})  -match \"{propertyName}\") -split {regex})");

            return result[1].BaseObject as string;
        }
    }
}
