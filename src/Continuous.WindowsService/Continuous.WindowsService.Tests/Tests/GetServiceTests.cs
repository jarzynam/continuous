﻿using System;
using System.Collections.Generic;
using System.Linq;
using Continuous.Management;
using Continuous.WindowsService.Model;
using Continuous.WindowsService.Model.Enums;
using Continuous.WindowsService.Shell;
using Continuous.WindowsService.Tests.TestHelpers;
using FluentAssertions;
using NUnit.Framework;
using ServiceInstaller = Continuous.WindowsService.Tests.TestHelpers.Installer.ServiceInstaller;


namespace Continuous.WindowsService.Tests.Tests
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
            service.ServiceDependencies.Should().BeEmpty();
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
        public void Get_Should_Return_Proper_StartMode_When_Is_AutomaticWithDelayedStart()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            var config = new WindowsServiceConfiguration
            {
                Name = serviceName,
                Path = _serviceInstaller.ServicePath,
                StartMode = WindowsServiceStartMode.AutomaticDelayedStart
            };

            _serviceInstaller.InstallService(config);

            // act
            var service = _shell.Get(serviceName);

            // assert
            service.StartMode.Should().Be(WindowsServiceStartMode.AutomaticDelayedStart);
        }

        [Test]
        public void Get_Should_Return_Proper_ServiceDependencies_When_Is_Collection()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);
            var serviceDependencies = new List<string>
            {
                _nameGenerator.GetRandomName(Prefix),
                _nameGenerator.GetRandomName(Prefix)
            };

            var config = new WindowsServiceConfiguration
            {
                Name = serviceName,
                Path = _serviceInstaller.ServicePath,
                ServiceDependencies = serviceDependencies
            };

            _serviceInstaller.InstallService(config);

            // act
            var service = _shell.Get(serviceName);

            // assert
            service.ServiceDependencies.Should().BeEquivalentTo(serviceDependencies);
        }

        [Test]
        public void Get_Should_Return_Updated_Description()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            var config = new WindowsServiceConfiguration
            {
                Name = serviceName,
                Path = _serviceInstaller.ServicePath,
                Description = "test"
            };

            _serviceInstaller.InstallService(config);

            // act
            var service = _shell.Get(serviceName);

            // assert
            service.Description.Should().Be(config.Description);
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
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            var config = new WindowsServiceConfiguration
            {
                Name = serviceName,
                Path = _serviceInstaller.ServicePath,
                Description = "new description",
                ServiceDependencies = new List<string>
                {
                    _nameGenerator.GetRandomName(Prefix),
                    _nameGenerator.GetRandomName(Prefix)
                },
                StartMode = WindowsServiceStartMode.AutomaticDelayedStart
            };

            _serviceInstaller.InstallService(config);

            // act
            var services = _shell.GetAll();
            
            // assert
            services.Should().NotBeNullOrEmpty();

            var testService = services.First(p => p.Name == serviceName);

            testService.AccountDomain.Should().Be(null);
            testService.AccountName.Should().Be("LocalSystem");
            testService.Description.Should().Be(config.Description);
            testService.DisplayName.Should().Be(serviceName);
            testService.ErrorControl.Should().Be(WindowsServiceErrorControl.Normal);
            testService.InteractWithDesktop.Should().Be(false);
            testService.Name.Should().Be(serviceName);
            testService.Path.Should().Be(_serviceInstaller.ServicePath);
            testService.ProcessId.Should().Be(0);
            testService.StartMode.Should().Be(WindowsServiceStartMode.AutomaticDelayedStart);
            testService.State.Should().Be(WindowsServiceState.Stopped);
            testService.Status.Should().Be(WindowsServiceStatus.Ok);
            testService.CanPause.Should().BeFalse();
            testService.CanStop.Should().BeFalse();
            testService.ExitCode.Should().Be(1077u);
            testService.ServiceDependencies.Should().BeEquivalentTo(config.ServiceDependencies);
        }

        [Test]
        public void GetStatate_Should_Fetch_Status()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            _serviceInstaller.InstallService(serviceName);

            // act
            var status = _shell.GetState(serviceName);

            // assert 
            status.Should().Be(WindowsServiceState.Stopped);
        }

        [Test]
        public void GetState_Should_Throw_When_Service_NotExists()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            // act
            Action act = () => _shell.GetState(serviceName);

            // assert 
            act.ShouldThrow<InvalidOperationException>();
        }

    }
}