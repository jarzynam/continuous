using System;
using Continuous.Management;
using Continuous.Test.WindowsService.Logic;
using Continuous.Test.WindowsService.Logic.Installer;
using Continuous.Test.WindowsService.TestHelpers;
using Continuous.WindowsService;
using Continuous.WindowsService.Model.Enums;
using Continuous.WindowsService.Shell;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.Test.WindowsService.Tests
{
    [TestFixture]
    public  class WaitForStateTests
    {
        private NameGenerator _nameGenerator;
        private IWindowsServiceShell _shell;
        private ServiceInstaller _serviceInstaller;
        private const string Prefix = "wfsTestService";
        
        [SetUp]
        public void SetUp()
        {
            _shell = new ContinuousManagementFactory().WindowsServiceShell();
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
            
            _serviceInstaller.InstallService(serviceName);
            ServiceHelper.StartService(serviceName);

            // act
            _shell.WaitForState(serviceName, WindowsServiceState.Running);

            // assert
            ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.Running);
        }

        [Test]
        public void WaitForState_Can_Wait_ForState_Paused()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            _serviceInstaller.InstallService(serviceName);
            ServiceHelper.StartService(serviceName);

            ServiceHelper.PauseService(serviceName, false);

            ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.PausePending);

            // act
            _shell.WaitForState(serviceName, WindowsServiceState.Paused, TimeSpan.FromMilliseconds(2000));

            // assert
            ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.Paused);
        }


        [Test]
        public void WaitForState_Can_Wait_ForState_Stopped()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            _serviceInstaller.InstallService(serviceName);
            ServiceHelper.StartService(serviceName);

            ServiceHelper.StopService(serviceName, false);
            ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.StopPending);


            // act
            _shell.WaitForState(serviceName, WindowsServiceState.Stopped);

            // assert
            ServiceHelper.GetState(serviceName).Should().Be(WindowsServiceState.Stopped);
        }

        [Test]
        public void WaitForState_Throws_Exception_When_ServiceNotExist()
        {
            // arrange 
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            // act
            Action act = () => _shell.WaitForState(serviceName, WindowsServiceState.ContinuePending);

            // assert
            act.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void WaitForState_Throws_Exception_When_Timeout()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            _serviceInstaller.InstallService(serviceName);
            ServiceHelper.StartService(serviceName);

            // act
            Action act = () => _shell.WaitForState(serviceName, WindowsServiceState.ContinuePending);

            // assert
            act.ShouldThrow<TimeoutException>();
        }


    }
}
