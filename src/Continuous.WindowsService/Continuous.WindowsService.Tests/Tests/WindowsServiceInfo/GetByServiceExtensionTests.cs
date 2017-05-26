using System;
using System.Collections.Generic;
using Continuous.WindowsService.Model;
using Continuous.WindowsService.Model.Enums;
using Continuous.WindowsService.Tests.TestHelpers;
using FluentAssertions;
using NUnit.Framework;
using ServiceInstaller = Continuous.WindowsService.Tests.TestHelpers.Installer.ServiceInstaller;


namespace Continuous.WindowsService.Tests.Tests.WindowsServiceInfo
{
    [TestFixture]
    public class GetByServiceExtensionTests
    {
        [SetUp]
        public void SetUp()
        {
            _serviceInstaller = new ServiceInstaller();

            _nameGenerator = new NameGenerator(Prefix);
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
        public void Get_Should_GetService()
        {
            // arrange
            var serviceName = _nameGenerator.RandomName;
            
            _serviceInstaller.InstallService(serviceName);
            
            // act
            var service = new Model.WindowsServiceInfo(serviceName);

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
            service.ServiceDependencies.Should().BeEmpty();
        }

        [Test]
        public void Get_Should_Return_AcceptStopAndStart_True_WhenService_IsRunning()
        {
            // arrange
            var serviceName = _nameGenerator.RandomName;

            _serviceInstaller.InstallService(serviceName);
            ServiceHelper.StartService(serviceName);

            // act
            var service = new Model.WindowsServiceInfo(serviceName);

            // assert
            service.State.Should().Be(WindowsServiceState.Running);
            service.CanPause.Should().BeTrue();
            service.CanStop.Should().BeTrue();
        }

        [Test]
        public void Get_Should_Return_Proper_StartMode_When_Is_AutomaticWithDelayedStart()
        {
            // arrange
            var serviceName = _nameGenerator.RandomName;

            var config = new WindowsServiceConfiguration
            {
                Name = serviceName,
                Path = _serviceInstaller.ServicePath,
                StartMode = WindowsServiceStartMode.AutomaticDelayedStart
            };

            _serviceInstaller.InstallService(config);

            // act
            var service = new Model.WindowsServiceInfo(serviceName);

            // assert
            service.StartMode.Should().Be(WindowsServiceStartMode.AutomaticDelayedStart);
        }

        [Test]
        public void Get_Should_Return_Proper_ServiceDependencies_When_Is_Collection()
        {
            // arrange
            var serviceName = _nameGenerator.RandomName;
            var serviceDependencies = new List<string>
            {
                _nameGenerator.RandomName,
                _nameGenerator.RandomName
            };

            var config = new WindowsServiceConfiguration
            {
                Name = serviceName,
                Path = _serviceInstaller.ServicePath,
                ServiceDependencies = serviceDependencies
            };

            _serviceInstaller.InstallService(config);

            // act
            var service = new Model.WindowsServiceInfo(serviceName);

            // assert
            service.ServiceDependencies.Should().BeEquivalentTo(serviceDependencies);
        }

        [Test]
        public void Get_Should_Return_Updated_Description()
        {
            // arrange
            var serviceName = _nameGenerator.RandomName;

            var config = new WindowsServiceConfiguration
            {
                Name = serviceName,
                Path = _serviceInstaller.ServicePath,
                Description = "test"
            };

            _serviceInstaller.InstallService(config);

            // act
            var service = new Model.WindowsServiceInfo(serviceName);

            // assert
            service.Description.Should().Be(config.Description);
        }

        [Test]
        public void Get_Should_Throw_When_ServiceNotExisting()
        {
            // arrange
            var serviceName = "fakeService";

            // act
            Action act = () =>{ var _ = new Model.WindowsServiceInfo(serviceName);};

            // assert
            act.ShouldThrow<InvalidOperationException>();
        }

    }
}