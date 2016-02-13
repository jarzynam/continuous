using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Rescuer.Management.WindowsService;
using Rescuer.Management.WindowsService.Shell;

namespace Rescuer.Management
{
    public class RescuerManagementModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WindowsServiceRescuer>().As<IWindowsServiceRescuer>();
            builder.RegisterType<WindowsServiceShell>().As<IWindowsServiceShell>();

        }
    }
}
