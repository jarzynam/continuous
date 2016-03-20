using Autofac;
using Rescuer.Management.Controller;
using Rescuer.Management.Rescuers.WindowsService;
using Rescuer.Management.Rescuers.WindowsService.Factory;
using Rescuer.Management.Rescuers.WindowsService.Shell;

namespace Rescuer.Management
{
    public class RescuerManagementModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WindowsServiceRescuer>().As<IWindowsServiceRescuer>();
            builder.RegisterType<WindowsServiceShell>().As<IWindowsServiceShell>();

            builder.RegisterType<RescuerController>().AsImplementedInterfaces();

            builder.RegisterType<WindowsServiceRescuerFactory>().As<IWindowsServiceRescuerFactory>();
        }
    }
}
