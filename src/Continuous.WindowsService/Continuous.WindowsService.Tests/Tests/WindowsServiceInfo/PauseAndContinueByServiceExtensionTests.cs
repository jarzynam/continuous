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
    public class PauseAndContinueByServiceExtensionTests
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
        
        private const string Prefix = "prTestService";

        [Test]
        public void Continue_Should_Resume_PausedService()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            var service = _serviceInstaller.InstallAndGetService(serviceName);

            ServiceHelper.StartService(serviceName);
            ServiceHelper.PauseService(serviceName);
            ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.Paused);

            // act
            service.Continue();

            // assert
            ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.Running);
        }

        [Test]
        public void Continue_Should_Not_Resume_RunningService()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            var service = _serviceInstaller.InstallAndGetService(serviceName);

            ServiceHelper.StartService(serviceName);
            ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.Running);

            // act
            service.Continue();

            // assert
            ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.Running);
        }

        [Test]
        public void Pause_Should_Not_Pause_PausedService()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            var service = _serviceInstaller.InstallAndGetService(serviceName);

            ServiceHelper.StartService(serviceName);
            ServiceHelper.PauseService(serviceName);
            ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.Paused);

            // act
            service.Pause();

            // assert
            ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.Paused);
        }

        [Test]
        public void Pause_Should_Pause_RunningService()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            var service = _serviceInstaller.InstallAndGetService(serviceName);

            ServiceHelper.StartService(serviceName);
            ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.Running);

            // act
            service.Pause();
        
            // assert
            ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.Paused);
        }
    }
}