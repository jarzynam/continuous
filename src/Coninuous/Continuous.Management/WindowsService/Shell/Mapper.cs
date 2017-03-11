using System;
using System.Linq;
using System.Management.Automation;
using Continuous.Management.Common.Extensions;
using Continuous.Management.WindowsService.Model;
using Continuous.Management.WindowsService.Model.Enums;

namespace Continuous.Management.WindowsService.Shell
{
    internal class Mapper
    {
        internal WindowsServiceInfo Map(PSObject result)
        {
            var info = new WindowsServiceInfo
            {
                Name = result.Properties["Name"].Value as string,
                DisplayName = result.Properties["DisplayName"].Value as string,
                Description = result.Properties["Description"].Value as string,
                ProcessId = (result.Properties["ProcessId"].Value as int?).GetValueOrDefault(),
                UserName = result.Properties["StartName"].Value as string,
                WindowsServiceType = (result.Properties["ServiceType"].Value as string).ToEnum<WindowsServiceType>(),
                StartMode = (result.Properties["StartMode"].Value as string).ToEnum<WindowsServiceStartMode>(),
                State = (result.Properties["State"].Value as string).ToEnum<WindowsServiceState>(),
                Status = (result.Properties["Status"].Value as string).ToEnum<WindowsServiceStatus>()
            };

            MapUser(result, info);

            return info;
        }

        private void MapUser(PSObject result, WindowsServiceInfo info)
        {
            var user = (result.Properties["StartName"].Value as string)?.Split('\\');

            if (user == null || !user.Any())
            {
                info.UserDomain = String.Empty;
                info.UserName = String.Empty;
            }
            else if (user.Length == 1)
            {
                info.UserName = user[0];
            }
            else if (user.Length > 1)
            {
                info.UserDomain = user[0];
                info.UserName = user[1];
            }
        }
    }
}