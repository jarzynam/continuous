using Continuous.Management;
using Continuous.WindowsService.Model.Enums;
using Continuous.WindowsService.Shell;
using Continuous.WindowsService.Tests.TestHelpers;
using FluentAssertions;
using NUnit.Framework;
using ServiceInstaller = Continuous.WindowsService.Tests.TestHelpers.Installer.ServiceInstaller;

namespace Continuous.WindowsService.Tests.Tests.WindowsServiceInfo
{
    [TestFixture]
    public class StartAndStopByServiceExtensionTests
    {
        [SetUp]
        public void SetUp()
        {
            _serviceInstaller = new ServiceInstaller();

            _nameGenerator = new NameGenerator();
        }

        [TearDown]
        public void TearDown()
        {
            _serviceInstaller.Dispose();
        }

        private ServiceInstaller _serviceInstaller;
        private NameGenerator _nameGenerator;
      
        private const string Prefix = "gsTestService";

        [Test]
        public void Start_Should_Start_StoppedService()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            var service = _serviceInstaller.InstallAndGetService(serviceName);

            ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.Stopped);

            // act
            service.Start();
           
            // assert
            ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.Running);
        }

        [Test]
        public void Start_Should_Not_Start_RunningService()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            var service = _serviceInstaller.InstallAndGetService(serviceName);

            ServiceHelper.StartService(serviceName);
            ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.Running);

            // act
            service.Start();
         
            // assert
            ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.Running);
        }

       

        [Test]
        public void Stop_Should_Not_Stop_StoppedService()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            var service = _serviceInstaller.InstallAndGetService(serviceName);

            ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.Stopped);

            // act
            service.Stop();
         
            // assert
           ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.Stopped);
        }

        [Test]
        public void Stop_Should_Stop_RunningService()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            var service = _serviceInstaller.InstallAndGetService(serviceName);

            ServiceHelper.StartService(serviceName);
            ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.Running);

            // act
            service.Stop();
      
            // assert
            ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.Stopped);
        }
    }
}