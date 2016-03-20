using Autofac;
using Rescuer.Management.WindowsService;

namespace Rescuer.Management.Factory.WindowsService
{
    public class WindowsServiceRescuerFactory : IWindowsServiceRescuerFactory
    {        
        private readonly ILifetimeScope _scope;

        public WindowsServiceRescuerFactory(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public IRescuer Create()
        {
            return _scope.Resolve<IWindowsServiceRescuer>();
        }
    }
}