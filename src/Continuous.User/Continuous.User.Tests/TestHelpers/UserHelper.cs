using System;
using System.Linq;
using Continuous.User.Users.Model;
#pragma warning disable 612

namespace Continuous.User.Tests.TestHelpers
{
    internal static class UserHelper
    {
        private const string SpecialAccountRegistry =
            @"HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon\SpecialAccounts\UserList";

        internal static void CreateUser(string userName, string userPassword)
        {
            ScriptInvoker.InvokeScript($"net user {userName} {userPassword} /add");
        }

        internal static void DeleteUser(string userName)
        {
            ScriptInvoker.InvokeScript($"net user {userName} /delete");
        }

        internal static UserModel GetUser(string userName)
        {
            var result = ScriptInvoker
                .InvokeScript($"Get-WMIObject -Class Win32_UserAccount -Filter \"Name = '{userName}'\"")
                .FirstOrDefault();

            if (result == null) return null;

            var model = new UserModel
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

        internal static DateTime GetPasswordLastSet(string userName)
        {
            return DateTime.Parse(GetUserProperty(userName, "Password last set"));
        }

        internal static DateTime? GetPasswordExpirationDate(string userName)
        {
            var result = GetUserProperty(userName, "Password expires");

            if (result == "Never") return null;

            return DateTime.Parse(result);
        }

        internal static TimeSpan GetPasswordBadAttemptsInterval(string userName)
        {
            var seconds = (int) GetPropertyFromAdsi(userName, "LockoutObservationInterval");

            return TimeSpan.FromSeconds(seconds);
        }

        internal static int GetPasswordMinLength(string userName)
        {
            return (int) GetPropertyFromAdsi(userName, "MinPasswordLength");
        }

        internal static int GetPasswordMaxBadAttempts(string userName)
        {
            return (int) GetPropertyFromAdsi(userName, "BadPasswordAttempts");
        }

        internal static int GetUserFlags(string userName)
        {
            return (int) GetPropertyFromAdsi(userName, "UserFlags");
        }

        internal static TimeSpan GetAutoUnlockInterval(string userName)
        {
            var seconds = (int) GetPropertyFromAdsi(userName, "AutoUnlockInterval");

            return TimeSpan.FromSeconds(seconds);
        }

        internal static DateTime? GetLastLogon(string userName)
        {
            var property = GetPropertyFromAdsi(userName, "LastLogin");

            return property as DateTime?;
        }

        internal static LocalUserCreateModel BuildLocalUser(string name)
        {
            return new LocalUserCreateModel
            {
                Name = name,
                FullName = "Test User 1",
                Description = "Delete this user after tests",
                Password = "Test123",
                AccountExpires = new DateTime(2018, 01, 01)
            };
        }

        internal static void SetUserFlag(string userName, int flag, bool value)
        {
            var userFlags = GetUserFlags(userName);

            userFlags = value
                ? userFlags | flag
                : userFlags & ~flag;

            SetPropertyFromAdsi(userName, "UserFlags", userFlags.ToString());
        }

        internal static bool GetPasswordExpired(string userName)
        {
            return (int) GetPropertyFromAdsi(userName, "PasswordExpired") > 0;
        }

        internal static void SetPasswordExipred(string userName, bool value)
        {
            SetPropertyFromAdsi(userName, "PasswordExpired", (value ? 1 : 0).ToString());
        }

        internal static void SetPassword(string userName, string newPassword)
        {
            var script = $"([ADSI] \"WinNT://./{userName}, user\").SetPassword({newPassword})";

            ScriptInvoker.InvokeScript(script);
        }

        internal static TimeSpan GetPassowrdAge(string userName)
        {
            var script = $"([ADSI] \"WinNT://./{userName}, user\").PasswordAge.Value";

            var result = (int) (ScriptInvoker.InvokeScript(script).FirstOrDefault()?.BaseObject ?? default(int));

            return TimeSpan.FromSeconds(result);
        }

        internal static string GetCurrentLoggedUserName()
        {
            var script = "(whoami).Split('\\')[1]";
            var result = ScriptInvoker.InvokeScript(script).FirstOrDefault()?.BaseObject;

            return result as string;
        }

        internal static bool GetUserVisibility(string userName)
        {
            var script = $"(Get-ItemProperty -Path '{SpecialAccountRegistry}' -Name {userName}) -eq $null";
            var result = ScriptInvoker.InvokeScript(script);

            return (bool) result.First().BaseObject;
        }

        internal static void AddSpecialAccountRegistry()
        {
            AddToRegistry(SpecialAccountRegistry);
        }

        internal static void RemoveSpecialAccountRegistry()
        {
            try
            {
                RemoveFromRegistry(SpecialAccountRegistry);
            }
            catch (Exception)
            {
                //ignore
            }
        }

        internal static void HideUser(string userName)
        {
            var script =
                $"New-ItemProperty -Path '{SpecialAccountRegistry}' -Name {userName} -Value 0 -PropertyType DWord -Force";

            ScriptInvoker.InvokeScript(script);
        }

        internal static void ShowUser(string userName)
        {
            var script = $"Remove-ItemProperty '{SpecialAccountRegistry}' -Name {userName} -Force";

            ScriptInvoker.InvokeScript(script);
        }

        public static string GetDescription(string userName)
        {
            return GetPropertyFromAdsi(userName, "Description") as string;
        }

        private static string GetUserProperty(string userName, string propertyName)
        {
            var propertyRegex = @"'\s{2,}'";
            var result = ScriptInvoker.InvokeScript(
                $"(((net user {userName})  -match \"{propertyName}\") -split {propertyRegex})");

            return result[1].BaseObject as string;
        }

        private static object GetPropertyFromAdsi(string userName, string propertyName)
        {
            var script = $"([ADSI] \"WinNT://./{userName}, user\").{propertyName}.Value";

            return ScriptInvoker.InvokeScript(script).FirstOrDefault()?.BaseObject;
        }

        private static void SetPropertyFromAdsi(string userName, string propertyName, string value)
        {
            var script = $"$user = [ADSI] (\"WinNT://./{userName}, user\");" +
                         $" $user.{propertyName}.Value = {value};" +
                         $" $user.SetInfo()";

            ScriptInvoker.InvokeScript(script);
        }

        private static void AddToRegistry(string path)
        {
            var script = $"New-Item '{path}' -Force";

            ScriptInvoker.InvokeScript(script);
        }

        private static void RemoveFromRegistry(string path)
        {
            var script = $"Remove-Item '{path}'";

            ScriptInvoker.InvokeScript(script);
        }

        
    }
}