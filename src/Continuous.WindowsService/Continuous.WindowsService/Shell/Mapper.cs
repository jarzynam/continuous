using System;
using System.Collections.Generic;
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
            var info = new WindowsServiceInfo();
            
            info.Name = result.Properties["Name"]?.Value as string;
            info.DisplayName = result.Properties["DisplayName"]?.Value as string;
            info.Description = result.Properties["Description"]?.Value as string;
            info.ProcessId = (result.Properties["ProcessId"]?.Value as int?).GetValueOrDefault();
            info.AccountName = result.Properties["StartName"]?.Value as string;
            info.Type = (result.Properties["ServiceType"]?.Value as string).ToEnum<WindowsServiceType>();
            info.StartMode = ((result.Properties["StartMode"]?.Value as string)?.Replace("Auto", "Automatic").ToEnum<WindowsServiceStartMode>()).GetValueOrDefault();
            info.State = (result.Properties["State"]?.Value as string).ToEnum<WindowsServiceState>();
            info.Status = (result.Properties["Status"]?.Value as string).ToEnum<WindowsServiceStatus>();
            info.ErrorControl = (result.Properties["ErrorControl"]?.Value as string).ToEnum<WindowsServiceErrorControl>();
            info.InteractWithDesktop = (result.Properties["DesktopInteract"]?.Value as bool?).GetValueOrDefault();
            info.Path = result.Properties["PathName"]?.Value as string;
            info.ExitCode = (result.Properties["ExitCode"]?.Value as UInt32?) .GetValueOrDefault();
            info.ServiceSpecificExitCode = (result.Properties["ServiceSpecificExitCode"]?.Value as UInt32?).GetValueOrDefault();
            info.CanPause = (result.Properties["AcceptPause"]?.Value as bool?).GetValueOrDefault();
            info.CanStop = (result.Properties["AcceptStop"]?.Value as bool?).GetValueOrDefault();
            info.ServiceDependencies = (result.Properties["ServiceDependencies"]?.Value as string[])?.ToList() ??
                                       new List<string>();
            

            if (info.StartMode == WindowsServiceStartMode.Automatic &&
                result.Properties["DelayedAutoStart"]?.Value as bool? == true)
            {
                info.StartMode = WindowsServiceStartMode.AutomaticDelayedStart;
            }

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

        public WindowsServiceState MapServiceState(PSObject result)
        {
            return (result.BaseObject as string).ToEnum<WindowsServiceState>();
        }
    }
}