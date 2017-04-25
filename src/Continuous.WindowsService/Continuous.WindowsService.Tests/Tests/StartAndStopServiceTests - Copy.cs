using System;
using System.ServiceProcess;
using Continuous.Management;
using Continuous.WindowsService.Shell;
using Continuous.WindowsService.Tests.TestHelpers;
using FluentAssertions;
using NUnit.Framework;
using ServiceInstaller = Continuous.WindowsService.Tests.TestHelpers.Installer.ServiceInstaller;

namespace Continuous.WindowsService.Tests.Tests
{
    [TestFixture]
    public class StartAndStopServiceTests
    {
        [SetUp]
        public void SetUp()
        {
            _shell = (new ContinuousManagementFactory()).WindowsServiceShell();
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
        
        private IWindowsServiceShell _shell;

        private const string Prefix = "gsTestService";

        [Test]
        public void Start_Should_Start_StoppedService()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            _serviceInstaller.InstallService(serviceName);

            _shell.GetStatus(serviceName).Should().Be(ServiceControllerStatus.Stopped);

            // act
            var result = _shell.Start(serviceName);

            // assert
            result.Should().BeTrue();

            ServiceHelper.GetStatus(serviceName).Should().Be(ServiceControllerStatus.Running);
        }

        [Test]
        public void Start_Should_Not_Start_RunningService()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            _serviceInstaller.InstallService(serviceName);

            ServiceHelper.StartService(serviceName);
            _shell.GetStatus(serviceName).Should().Be(ServiceControllerStatus.Running);

            // act
            var result = _shell.Start(serviceName);

            // assert
            result.Should().BeFalse();

            ServiceHelper.GetStatus(serviceName).Should().Be(ServiceControllerStatus.Running);
        }

        [Test]
        public void Start_Should_Throw_If_Service_NotExist()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            // act
            Action act = () => _shell.Start(serviceName);

            // assert
            act.ShouldThrow<InvalidOperationException>();
        }


        [Test]
        public void Stop_Should_Not_Stop_StoppedService()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            _serviceInstaller.InstallService(serviceName);

            _shell.GetStatus(serviceName).Should().Be(ServiceControllerStatus.Stopped);

            // act
            var result = _shell.Stop(serviceName);

            // assert
            result.Should().BeFalse();

            ServiceHelper.GetStatus(serviceName).Should().Be(ServiceControllerStatus.Stopped);
        }

        [Test]
        public void Stop_Should_Stop_RunningService()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            _serviceInstaller.InstallService(serviceName);

            ServiceHelper.StartService(serviceName);
            _shell.GetStatus(serviceName).Should().Be(ServiceControllerStatus.Running);

            // act
            var result = _shell.Stop(serviceName);

            // assert
            result.Should().BeTrue();

            ServiceHelper.GetStatus(serviceName).Should().Be(ServiceControllerStatus.Stopped);
        }

        [Test]
        public void Stop_Should_Throw_If_Service_NotExist()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            // act
            Action act = () => _shell.Start(serviceName);

            // assert
            act.ShouldThrow<InvalidOperationException>();
        }
    }
}