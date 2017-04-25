﻿using System;
using System.ServiceProcess;
using Continuous.Management;
using Continuous.Test.WindowsService.Logic;
using Continuous.Test.WindowsService.TestHelpers;
using Continuous.WindowsService;
using Continuous.WindowsService.Shell;
using FluentAssertions;
using NUnit.Framework;

using ServiceInstaller = Continuous.Test.WindowsService.Logic.Installer.ServiceInstaller;

namespace Continuous.Test.WindowsService.Tests
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

            _shell.GetStatus(serviceName).Should().Be(ServiceControllerStatus.Paused);

            // act
            var result = _shell.Continue(serviceName);

            // assert
            result.Should().BeTrue();

            ServiceHelper.GetStatus(serviceName).Should().Be(ServiceControllerStatus.Running);
        }

        [Test]
        public void Continue_Should_Not_Resume_RunningService()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            _serviceInstaller.InstallService(serviceName);
            ServiceHelper.StartService(serviceName);
            
            _shell.GetStatus(serviceName).Should().Be(ServiceControllerStatus.Running);

            // act
            var result = _shell.Continue(serviceName);

            // assert
            result.Should().BeFalse();

            ServiceHelper.GetStatus(serviceName).Should().Be(ServiceControllerStatus.Running);
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

            _shell.GetStatus(serviceName).Should().Be(ServiceControllerStatus.Paused);

            // act
            var result = _shell.Pause(serviceName);

            // assert
            result.Should().BeFalse();

            ServiceHelper.GetStatus(serviceName).Should().Be(ServiceControllerStatus.Paused);
        }

        [Test]
        public void Pause_Should_Pause_RunningService()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            _serviceInstaller.InstallService(serviceName);

            ServiceHelper.StartService(serviceName);
            _shell.GetStatus(serviceName).Should().Be(ServiceControllerStatus.Running);

            // act
            var result = _shell.Pause(serviceName);

            // assert
            result.Should().BeTrue();

            ServiceHelper.GetStatus(serviceName).Should().Be(ServiceControllerStatus.Paused);
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