using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.ServiceProcess;
using Continuous.Compability.WindowsService.Logic;
using Continuous.WindowsService.Model;
using Continuous.WindowsService.Model.Enums;
using FluentAssertions;
using NUnit.Framework;

using ServiceInstaller = Continuous.Compability.WindowsService.Logic.ServiceInstaller;

namespace Continuous.Compability.WindowsService.Tests
{
    [TestFixture]
    public class InstallServiceTests
    {
        private ServiceInstaller _intaller;
        private NameGenerator _nameGenerator;
        private ServiceLogReader _serviceLogReader;

        private const string Prefix = "testService";

        [SetUp]
        public void SetUp()
        {
            _intaller = new ServiceInstaller();
            _nameGenerator = new NameGenerator();
            _serviceLogReader = new ServiceLogReader();
        }

        [TearDown]
        public void TearDown()
        {
            _intaller.Dispose();
        }

        [Test]
        public void Install_Should_Install_When_Ok()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            // act
            _intaller.InstallService(name);

            // assert
            var serviceController = new ServiceController(name);
            serviceController.ServiceName.Should().Be(name);
        }

        [Test]
        public void Install_Should_Throw_Exectipn_When_Path_Is_Incorrect()
        {
            // arrange 
            var name = _nameGenerator.GetRandomName(Prefix);
            _intaller.ServicePath = "fakePath";

            // act
            Action act =() =>  _intaller.InstallService(name);

            // assert
            act.ShouldThrow<FileNotFoundException>();
        }

      

        [Test]
        public void Install_Should_Install_When_Provide_DefaultConfig()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var config = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _intaller.ServicePath,
                DisplayName = name
            };

            // act 
            _intaller.InstallService(config);

            // assert
            var serviceController = new ServiceController(name);
            serviceController.ServiceName.Should().Be(name);
        }

        [Test]
        public void Install_Should_Throw_When_Path_IsInvalid()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var config = new WindowsServiceConfiguration
            {
                Name = name,
                Path = "fakePath",
                DisplayName = name
            };

            // act 
            Action act = () => _intaller.InstallService(config);

            // assert
            act.ShouldThrow<FileNotFoundException>();
        }

        [Test]
        public void Install_Should_AddCorrectParameters_Path()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var config = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _intaller.ServicePath,
                DisplayName = name
            };

            // act 
            _intaller.InstallService(config);

            // assert
            var path = GetServiceHelper.GetPath(name);
            var displayName = GetServiceHelper.GetDisplayName(name);
            var account = GetServiceHelper.GetAccount(name);
            var starMode = GetServiceHelper.GetStartMode(name);
            var serviceType = GetServiceHelper.GetServiceType(name);
            var errorControl = GetServiceHelper.GetErrorControl(name);

            path.Should().Be(config.Path);
            displayName.Should().Be(config.DisplayName);
            account.Should().Be("LocalSystem");
            starMode.Should().Be((int) config.StartMode);
            serviceType.Should().Be((int) config.Type);
            errorControl.Should().Be((int) config.ErrorControl);
        }

        [Test]
        public void Install_Should_Change_ErrorControl_ToCritical()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var config = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _intaller.ServicePath,
                DisplayName = name,
                ErrorControl = WindowsServiceErrorControl.Critical
            };

            // act 
            _intaller.InstallService(config);

            // assert
            var errorControl = GetServiceHelper.GetErrorControl(name);

            errorControl.Should().Be((int) config.ErrorControl);
        }

        [Test]
        public void Install_Should_Change_ErrorControl_ToIgnore()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var config = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _intaller.ServicePath,
                DisplayName = name,
                ErrorControl = WindowsServiceErrorControl.Ignore
            };

            // act 
            _intaller.InstallService(config);

            // assert
            var errorControl = GetServiceHelper.GetErrorControl(name);

            errorControl.Should().Be((int)config.ErrorControl);
        }


        [Test]
        public void Install_Should_Change_ErrorControl_ToNormal()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var config = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _intaller.ServicePath,
                DisplayName = name,
                ErrorControl = WindowsServiceErrorControl.Normal
            };

            // act 
            _intaller.InstallService(config);

            // assert
            var errorControl = GetServiceHelper.GetErrorControl(name);

            errorControl.Should().Be((int)config.ErrorControl);
        }

        [Test]
        public void Install_Should_Change_ErrorControl_ToSevere()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var config = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _intaller.ServicePath,
                DisplayName = name,
                ErrorControl = WindowsServiceErrorControl.Severe
            };

            // act 
            _intaller.InstallService(config);

            // assert
            var errorControl = GetServiceHelper.GetErrorControl(name);

            errorControl.Should().Be((int)config.ErrorControl);
        }

        [Test]
        public void Install_Should_Change_ServiceType_To_ShareProcess()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var config = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _intaller.ServicePath,
                DisplayName = name,
                Type = WindowsServiceType.ShareProcess
            };

            // act 
            _intaller.InstallService(config);

            // assert
            var errorControl = GetServiceHelper.GetServiceType(name);

            errorControl.Should().Be((int)config.Type);
        }

        //todo: add tests for service types: drivers, interactive proccess 

        [Test]
        public void Install_Should_Change_StartMode_To_Disabled()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var config = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _intaller.ServicePath,
                DisplayName = name,
                StartMode = WindowsServiceStartMode.Disabled
            };

            // act 
            _intaller.InstallService(config);

            // assert
            var startMode = GetServiceHelper.GetStartMode(name);

            startMode.Should().Be((int)config.StartMode);
        }

        [Test]
        public void Install_Should_Change_StartMode_To_Manual()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var config = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _intaller.ServicePath,
                DisplayName = name,
                StartMode = WindowsServiceStartMode.Manual
            };

            // act 
            _intaller.InstallService(config);

            // assert
            var startMode = GetServiceHelper.GetStartMode(name);

            startMode.Should().Be((int)config.StartMode);
        }

        [Test]
        public void Install_Should_Change_StartMode_To_Automatic()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var config = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _intaller.ServicePath,
                DisplayName = name,
                StartMode = WindowsServiceStartMode.Automatic
            };

            // act 
            _intaller.InstallService(config);

            // assert
            var startMode = GetServiceHelper.GetStartMode(name);

            startMode.Should().Be((int)config.StartMode);
        }

        //todo: add test for start mode: boot, system


        [Test]
        public void Install_Should_Change_InteractiveWithDesktop_ToTrue()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            var config = new WindowsServiceConfiguration
            {
                Name = name,
                Path = _intaller.ServicePath,
                DisplayName = name,
                InteractWithDesktop = true
            };

            // act 
            _intaller.InstallService(config);

            // assert
            var type = GetServiceHelper.GetServiceType(name);
            var interactiveWithDekstopFlag = 0x100;
            
            type.Should().Be((int) config.Type | interactiveWithDekstopFlag);
        }


        //todo: add change account tests
    }
}
