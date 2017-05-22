using System;
using Continuous.WindowsService.Model.Enums;
using Continuous.WindowsService.Tests.TestHelpers;
using Continuous.WindowsService.Tests.TestHelpers.Installer;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.WindowsService.Tests.Tests.WindowsServiceInfo
{
    [TestFixture]
    public  class WaitForStateByServiceExtensionTests
    {
        private NameGenerator _nameGenerator;
     
        private ServiceInstaller _serviceInstaller;
        private const string Prefix = "wfsTestService";
        
        [SetUp]
        public void SetUp()
        {
            _nameGenerator = new NameGenerator();

            _serviceInstaller = new ServiceInstaller();

            _serviceInstaller.ServicePath = _serviceInstaller.ServicePath.Replace("BasicService", "BasicService2");
        }

        [TearDown]
        public void TearDown()
        {
            _serviceInstaller.Dispose();
        }

        [Test]
        public void WaitForState_Can_Wait_ForState_Running()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);
            
            var service = _serviceInstaller.InstallAndGetService(serviceName);
            ServiceHelper.StartService(serviceName);

            // act
            service.WaitForState(WindowsServiceState.Running);

            // assert
            ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.Running);
        }

        [Test]
        public void WaitForState_Can_Wait_ForState_Paused()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            var service = _serviceInstaller.InstallAndGetService(serviceName);

            ServiceHelper.StartService(serviceName);
            ServiceHelper.PauseService(serviceName, false);

            ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.PausePending);

            // act
            service.WaitForState(WindowsServiceState.Paused);

            // assert
            ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.Paused);
        }


        [Test]
        public void WaitForState_Can_Wait_ForState_Stopped()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

           var service =   _serviceInstaller.InstallAndGetService(serviceName);

            ServiceHelper.StartService(serviceName);
            ServiceHelper.StopService(serviceName);

            // act
            service.WaitForState(WindowsServiceState.Stopped);

            // assert
            ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.Stopped);
        }


        [Test]
        public void WaitForState_Throws_Exception_When_Timeout()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            var service = _serviceInstaller.InstallAndGetService(serviceName);

            ServiceHelper.StartService(serviceName);

            // act
            Action act = () => service.WaitForState(WindowsServiceState.ContinuePending);

            // assert
            act.ShouldThrow<TimeoutException>();
        }


    }
}
