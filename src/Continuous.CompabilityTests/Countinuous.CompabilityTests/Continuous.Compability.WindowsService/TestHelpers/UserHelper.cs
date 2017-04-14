using Continuous.Compability.WindowsService.Logic;

namespace Continuous.Compability.WindowsService.TestHelpers
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

    }
}
