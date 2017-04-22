using System;
using System.Linq;
using System.Management.Automation;
using Continuous.Management.Common.Extensions;
using Continuous.WindowsService.Model;
using Continuous.WindowsService.Model.Enums;

namespace Continuous.WindowsService.Shell
{
    internal class Mapper
    {
        internal WindowsServiceInfo Map(PSObject result)
        {
            var info = new WindowsServiceInfo
            {
                Name = result.Properties["Name"]?.Value as string,
                DisplayName = result.Properties["DisplayName"]?.Value as string,
                Description = result.Properties["Description"]?.Value as string,
                ProcessId = (result.Properties["ProcessId"]?.Value as int?).GetValueOrDefault(),
                AccountName = result.Properties["StartName"]?.Value as string,
                Type = (result.Properties["ServiceType"]?.Value as string).ToEnum<WindowsServiceType>(),
                StartMode = ((result.Properties["StartMode"]?.Value as string)?.Replace("Auto", "Automatic").ToEnum<WindowsServiceStartMode>()).GetValueOrDefault(),
                State = (result.Properties["State"]?.Value as string).ToEnum<WindowsServiceState>(),
                Status = (result.Properties["Status"]?.Value as string).ToEnum<WindowsServiceStatus>(),
                ErrorControl = (result.Properties["ErrorControl"]?.Value as string).ToEnum<WindowsServiceErrorControl>(),
                InteractWithDesktop = (result.Properties["DesktopInteract"]?.Value as bool?).GetValueOrDefault(),
                Path = result.Properties["PathName"]?.Value as string,
                ExitCode = (result.Properties["ExitCode"]?.Value as UInt32?) .GetValueOrDefault(),
                ServiceSpecificExitCode = (result.Properties["ServiceSpecificExitCode"]?.Value as UInt32?).GetValueOrDefault()
            };

            MapUser(result, info);

            return info;
        }

        private void MapUser(PSObject result, WindowsServiceInfo info)
        {
            var user = (result.Properties["StartName"]?.Value as string)?.Split('\\');

            if (user == null || !user.Any())
            {
                info.AccountDomain = String.Empty;
                info.AccountName = String.Empty;
            }
            else if (user.Length == 1)
            {
                info.AccountName = user[0];
            }
            else if (user.Length > 1)
            {
                info.AccountDomain = user[0];
                info.AccountName = user[1];
            }
        }
    }
}