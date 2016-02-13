using System;
using Autofac;
using Rescuer.Management.WindowsService;

namespace Rescuer.Management.Factory
{
    public class WindowsServiceRescuerFactory : IRescuerFactory
    {
        private readonly IContainer _container;

        public WindowsServiceRescuerFactory(IContainer container)
        {
            _container = container;
        }

        public IRescuer Create()
        {
            return _container.Resolve<IWindowsServiceRescuer>();
        }
    }
}