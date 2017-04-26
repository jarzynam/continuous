using System;
using System.ServiceProcess;
using Continuous.Management;
using Continuous.WindowsService.Model.Enums;
using Continuous.WindowsService.Shell;
using Continuous.WindowsService.Tests.TestHelpers;
using FluentAssertions;
using NUnit.Framework;
using ServiceInstaller = Continuous.WindowsService.Tests.TestHelpers.Installer.ServiceInstaller;

namespace Continuous.WindowsService.Tests.Tests
{
    [TestFixture]
    public class PauseAndContinueServiceTests
    {
        [SetUp]
        public void SetUp()
        {
            _shell = new ContinuousManagementFactory().WindowsServiceShell();
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

        private const string Prefix = "prTestService";

        [Test]
        public void Continue_Should_Resume_PausedService()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            _serviceInstaller.InstallService(serviceName);
            ServiceHelper.StartService(serviceName);
            ServiceHelper.PauseService(serviceName);

            _shell.GetState(serviceName).Should().Be(WindowsServiceState.Paused);

            // act
            var result = _shell.Continue(serviceName);

            // assert
            result.Should().BeTrue();

            ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.Running);
        }

        [Test]
        public void Continue_Should_Not_Resume_RunningService()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            _serviceInstaller.InstallService(serviceName);

            ServiceHelper.StartService(serviceName);

            ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.Running);
           
            // act
            var result = _shell.Continue(serviceName);
        
            // assert
            result.Should().BeFalse();

            ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.Running);
        }

        [Test]
        public void Continue_Should_Throw_If_Service_NotExist()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            // act
            Action act = () => _shell.Continue(serviceName);

            // assert
            act.ShouldThrow<InvalidOperationException>();
        }


        [Test]
        public void Pause_Should_Not_Pause_PausedService()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            _serviceInstaller.InstallService(serviceName);
            ServiceHelper.StartService(serviceName);
            ServiceHelper.PauseService(serviceName);

            _shell.GetState(serviceName).Should().Be(WindowsServiceState.Paused);

            // act
            var result = _shell.Pause(serviceName);
   
            // assert
            result.Should().BeFalse();

            ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.Paused);
           
        }

        [Test]
        public void Pause_Should_Pause_RunningService()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            _serviceInstaller.InstallService(serviceName);

            ServiceHelper.StartService(serviceName);

            _shell.GetState(serviceName).Should().Be(WindowsServiceState.Running);

            // act
            var result = _shell.Pause(serviceName);
        
            // assert
            result.Should().BeTrue();

            ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.Paused);
        }

        [Test]
        public void Pause_Should_Throw_If_Service_NotExist()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            // act
            Action act = () => _shell.Pause(serviceName);

            // assert
            act.ShouldThrow<InvalidOperationException>();
        }
    }
}