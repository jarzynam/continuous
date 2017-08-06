using System.Linq;

namespace Continuous.WindowsService.Tests.TestHelpers
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

        public static void AssingnUserToAdministators(string userName)
        {
            var getAdminGroupNameScript = "(Get-WMIObject -class Win32_Group -Filter \"SID= 'S-1-5-32-544' and LocalAccount= '$true'\").Name";
            var adminGroupName = ScriptInvoker.InvokeScript(getAdminGroupNameScript).FirstOrDefault()?.BaseObject as string;

            var assignUserToGroupScript = $"net localgroup {adminGroupName} {userName} /add";

            ScriptInvoker.InvokeScript(assignUserToGroupScript);

        }

    }
}
