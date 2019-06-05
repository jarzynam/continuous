using System;
using System.DirectoryServices.AccountManagement;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace Continuous.User.Tests.TestHelpers.Installer
{
  public static class WindowsUserProfile
  {
      [DllImport("UserEnv.dll", CharSet = CharSet.Unicode)]
      private static extern int CreateProfile(
          [In] string pszUserSid,
          [In] string pszUserName,
          StringBuilder pszProfilePath,
          int cchProfilePath);

      [DllImport("UserEnv.dll", CharSet = CharSet.Unicode, ExactSpelling = false, SetLastError = true)]
      private static extern bool DeleteProfile(string sidString, string profilePath, string computerName);

      public static SecurityIdentifier Create(string userName)
      {
          using (var principalContext = new PrincipalContext(ContextType.Machine, Environment.MachineName))
          {
              var userPrincipal = UserPrincipal.FindByIdentity(principalContext, IdentityType.SamAccountName, userName);

              CreateProfile(userPrincipal);

              return userPrincipal?.Sid;
          }
      }

      public static void Delete(SecurityIdentifier userSecurityId)
      {
          DeleteProfile(userSecurityId.ToString(), null, null);
      }

      private static void CreateProfile(UserPrincipal userPrincipal)
      {
          int MaxPath = 240;
          var pathBuf = new StringBuilder(MaxPath);

          CreateProfile(userPrincipal.Sid.ToString(), userPrincipal.SamAccountName, pathBuf, pathBuf.Capacity);
      }

  }
}