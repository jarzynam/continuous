using NUnit.Framework;
using Rescuer.Management.Controller;
using Rescuer.Management.Rescuers;

namespace Rescuer.Management.Tests
{
    [TestFixture]
    class RescuerControllerFactoryTests
    {
        [Test]
        public void Can_Create_Controller_With_Default_RescuerType_Test()
        {
            using (var controllerFactory = new RescuerControllerFactory())
            {
                var controller = controllerFactory.Create();

                Assert.IsNotNull(controller);                
            }
        }

        [Test]
        public void Can_Create_Controller_With_WindowsService_RescuerType_Test()
        {
            using (var controllerFactory = new RescuerControllerFactory(RescuerType.WindowsServiceRescuer))
            {
                var controller = controllerFactory.Create();

                Assert.IsNotNull(controller);
            }
        }        
    }
}
