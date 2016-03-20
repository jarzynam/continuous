using Autofac;
using NUnit.Framework;
using Rescuer.Management.Factory;
using Rescuer.Management.Factory.WindowsService;
using Rescuer.Management.WindowsService;

namespace Rescuer.Management.Tests
{
    [TestFixture]
    class RescuerManagementModuleTests
    {
        [Test]
        public void Can_Build_RescuerManagementModule_Test()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<RescuerManagementModule>();

            using (var scope = builder.Build().BeginLifetimeScope())
            {
                IWindowsServiceRescuer rescuer = null;

                Assert.DoesNotThrow(() =>
                {
                    rescuer = scope.Resolve<IWindowsServiceRescuer>();
                }, "Cant load all components to windows service rescuer" );
                    

                Assert.IsNotNull(rescuer, "rescuer instance cant be null");
            }
        }

        [Test]
        public void Can_Get_WindowsServiceRescuer_FromFactory_Test()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<RescuerManagementModule>();
            
            using (var scope = builder.Build().BeginLifetimeScope())
            {
                var rescuer = scope.Resolve<IWindowsServiceRescuerFactory>();

                Assert.AreEqual(typeof(WindowsServiceRescuerFactory), rescuer.GetType());
            }
        }
    }
}
