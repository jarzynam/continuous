using Autofac;
using NUnit.Framework;
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

            using (var container = builder.Build())
            {
                IWindowsServiceRescuer rescuer = null;

                Assert.DoesNotThrow(() =>
                {
                    rescuer = container.Resolve<IWindowsServiceRescuer>();
                }, "Cant load all components to windows service rescuer" );
                    

                Assert.IsNotNull(rescuer, "rescuer instance cant be null");
            }
        }
    }
}
