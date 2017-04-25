using System;
using System.ServiceProcess;
using Continuous.Management;
using Continuous.Test.WindowsService.Logic;
using Continuous.Test.WindowsService.TestHelpers;
using Continuous.WindowsService;
using Continuous.WindowsService.Model.Enums;
using Continuous.WindowsService.Shell;
using FluentAssertions;
using NUnit.Framework;
using ServiceInstaller = Continuous.Test.WindowsService.Logic.Installer.ServiceInstaller;

namespace Continuous.Test.WindowsService.Tests
{
    [TestFixture]
    public class GetServiceTest
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
        public void Get_Should_GetService()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);
            
            _serviceInstaller.InstallService(serviceName);
            
            // act
            var service = _shell.Get(serviceName);

            // assert
            service.AccountDomain.Should().Be(null);
            service.AccountName.Should().Be("LocalSystem");
            service.Description.Should().Be(null);
            service.DisplayName.Should().Be(serviceName);
            service.ErrorControl.Should().Be(WindowsServiceErrorControl.Normal);
            service.InteractWithDesktop.Should().Be(false);
            service.Name.Should().Be(serviceName);
            service.Path.Should().Be(_serviceInstaller.ServicePath);
            service.ProcessId.Should().Be(0);
            service.StartMode.Should().Be(WindowsServiceStartMode.Automatic);
            service.State.Should().Be(WindowsServiceState.Stopped);
            service.Status.Should().Be(WindowsServiceStatus.Ok);
            service.CanPause.Should().BeFalse();
            service.CanStop.Should().BeFalse();

            service.ExitCode.Should().Be(1077u);
        }

        [Test]
        public void Get_Should_Return_AcceptStopAndStart_True_WhenService_IsRunning()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            _serviceInstaller.InstallService(serviceName);
            ServiceHelper.StartService(serviceName);

            // act
            var service = _shell.Get(serviceName);

            // assert
            service.State.Should().Be(WindowsServiceState.Running);
            service.CanPause.Should().BeTrue();
            service.CanStop.Should().BeTrue();
        }

        [Test]
        public void Get_Should_Return_Null_When_ServiceNotExisting()
        {
            // arrange
            var serviceName = "fakeService";

            // act
            var service = _shell.Get(serviceName);

            // assert
            service.Should().BeNull();
        }

        [Test]
        public void GetAll_Should_Fetch_AllServices()
        {
            // act
            var services = _shell.GetAll();

            // assert
            services.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void GetStatus_Should_Fetch_Status()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            _serviceInstaller.InstallService(serviceName);

            // act
            var status = _shell.GetStatus(serviceName);

            // assert 
            status.Should().Be(ServiceControllerStatus.Stopped);
        }

        [Test]
        public void GetStatus_Should_Throw_When_Service_NotExists()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            // act
            Action act = () => _shell.GetStatus(serviceName);

            // assert 
            act.ShouldThrow<InvalidOperationException>();
        }

    }
}