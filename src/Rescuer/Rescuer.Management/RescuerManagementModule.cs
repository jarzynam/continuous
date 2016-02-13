using Autofac;
using Rescuer.Management.Factory;
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

            builder.RegisterType<IRescuerController>().AsImplementedInterfaces();

            builder.RegisterType<WindowsServiceRescuerFactory>()
                .AsImplementedInterfaces()
                .Keyed<RescuerFactoryType>(RescuerFactoryType.WindowsServiceRescuer);
        }
    }
}
