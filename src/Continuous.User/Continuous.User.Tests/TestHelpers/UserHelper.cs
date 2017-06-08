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

        public static DateTime GetPasswordLastSet(string userName)
        {
            return DateTime.Parse(GetUserProperty(userName, "Password last set"));
        }
        
        public static DateTime GetPasswordExpirationDate(string userName)
        {
            return DateTime.Parse(GetUserProperty(userName, "Password expires"));
        }

        public static TimeSpan GetPasswordBadAttemptsInterval(string userName)
        {
            var seconds = (int) GetPropertyFromAdsi(userName, "LockoutObservationInterval");

            return TimeSpan.FromSeconds(seconds);
        }

        public static int GetPasswordMinLength(string userName)
        {
            return (int) GetPropertyFromAdsi(userName, "MinPasswordLength");
        }

        public static int GetPasswordMaxBadAttempts(string userName)
        {
            return (int) GetPropertyFromAdsi(userName, "BadPasswordAttempts");
        }

        public static int GetUserFlags(string userName)
        {
            return (int) GetPropertyFromAdsi(userName, "UserFlags");
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
            var propertyRegex = @"'\s{2,}'";
            var result = ScriptInvoker.InvokeScript($"(((net user {userName})  -match \"{propertyName}\") -split {propertyRegex})");

            return result[1].BaseObject as string;
        }

        private static object GetPropertyFromAdsi(string userName, string propertyName)
        {
            var script = $"([ADSI] \"WinNT://./{userName}, user\").{propertyName}.Value";

            return ScriptInvoker.InvokeScript(script).FirstOrDefault()?.BaseObject;
        }
    }
}
