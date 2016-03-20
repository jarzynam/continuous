using Autofac;
using Rescuer.Management.Factory;
using Rescuer.Management.Factory.WindowsService;
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

            builder.RegisterType<RescuerController>().AsImplementedInterfaces();

            builder.RegisterType<WindowsServiceRescuerFactory>().As<IWindowsServiceRescuerFactory>();
        }
    }
}
