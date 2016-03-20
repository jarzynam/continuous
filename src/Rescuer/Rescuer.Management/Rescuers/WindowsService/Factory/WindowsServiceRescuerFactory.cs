using Autofac;

namespace Rescuer.Management.Rescuers.WindowsService.Factory
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