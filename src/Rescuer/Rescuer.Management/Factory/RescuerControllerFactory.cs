using System;
using Autofac;
using Rescuer.Management.Factory.WindowsService;

namespace Rescuer.Management.Factory
{
    public class RescuerControllerFactory : IDisposable
    {        
        private readonly ILifetimeScope _scope;        
        private readonly RescuerType _rescuerType;

        public RescuerControllerFactory() : this(RescuerType.WindowsServiceRescuer)
        {
            
        }

        public RescuerControllerFactory(RescuerType type)
        {            
            var builder = new ContainerBuilder();

            builder.RegisterModule<RescuerManagementModule>();

            _scope = builder.Build().BeginLifetimeScope();

            _rescuerType = type;
        }

        private IRescuerFactory GetFactory()
        {
            switch (_rescuerType)
            {
                case RescuerType.WindowsServiceRescuer:
                    return _scope.Resolve<IWindowsServiceRescuerFactory>();

                default:
                    throw new ArgumentException($"Invalid rescuer factory type: {_rescuerType}");
            }
        }

        public IRescuerController Create()
        {
            var factory = GetFactory();
            var controller = _scope.Resolve<IRescuerController>(new NamedParameter("factory", factory));

            return controller;
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}
