using System;
using System.IO;
using System.ServiceProcess;
using Continuous.Test.WindowsService.Logic;
using Continuous.Test.WindowsService.Logic.Installer;
using Continuous.Test.WindowsService.TestHelpers;
using Continuous.WindowsService.Model;
using Continuous.WindowsService.Model.Enums;
using FluentAssertions;
using NUnit.Framework;
using ServiceInstaller = Continuous.Test.WindowsService.Logic.Installer.ServiceInstaller;

namespace Continuous.Test.WindowsService.Tests
{
    [TestFixture]
    public class InstallServiceTests
    {
        private ServiceInstaller _serviceInstaller;
        private NameGenerator _nameGenerator;
        private UserInstaller _userInstaller;

        private const string Prefix = "cTestService";

        [SetUp]
        public void SetUp()
        {
            _serviceInstaller = new ServiceInstaller();
            _userInstaller = new UserInstaller();

            _nameGenerator = new NameGenerator();
        }

        [TearDown]
        public void TearDown()
        {
            _serviceInstaller.Dispose();
            _userInstaller.Dispose();
        }

        [Test]
        public void Install_Should_Install_When_Ok()
        {
            // arrange
            var name = _nameGenerator.GetRandomName(Prefix);

            // act
            _serviceInstaller.InstallService(name);

            // assert
            var serviceController = new ServiceController(name);
            serviceController.ServiceName.Should().Be(name);
        }

        [Test]
        public void Install_Should_Throw_Exectipn_When_Path_Is_Incorrect()
        {
            // arrange 
            var name = _nameGenerator.GetRandomName(Prefix);
            _serviceInstaller.ServicePath = "fakePath";

            // act
            Action act =() =>  _serviceInstaller.InstallService(name);

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
                Path = _serviceInstaller.ServicePath,
                DisplayName = name
            };

            // act 
            _serviceInstaller.InstallService(config);

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
            Action act = () => _serviceInstaller.InstallService(config);

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
                Path = _serviceInstaller.ServicePath,
                DisplayName = name
            };

            // act 
            _serviceInstaller.InstallService(config);

            // assert
            var path = ServiceHelper.GetPath(name);
            var displayName = ServiceHelper.GetDisplayName(name);
            var account = ServiceHelper.GetAccount(name);
            var starMode = ServiceHelper.GetStartMode(name);
            var serviceType = ServiceHelper.GetServiceType(name);
            var errorControl = ServiceHelper.GetErrorControl(name);

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
                Path = _serviceInstaller.ServicePath,
                DisplayName = name,
                ErrorControl = WindowsServiceErrorControl.Critical
            };

            // act 
            _serviceInstaller.InstallService(config);

            // assert
            var errorControl = ServiceHelper.GetErrorControl(name);

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
                Path = _serviceInstaller.ServicePath,
                DisplayName = name,
                ErrorControl = WindowsServiceErrorControl.Ignore
            };

            // act 
            _serviceInstaller.InstallService(config);

            // assert
            var errorControl = ServiceHelper.GetErrorControl(name);

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
                Path = _serviceInstaller.ServicePath,
                DisplayName = name,
                ErrorControl = WindowsServiceErrorControl.Normal
            };

            // act 
            _serviceInstaller.InstallService(config);

            // assert
            var errorControl = ServiceHelper.GetErrorControl(name);

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
                Path = _serviceInstaller.ServicePath,
                DisplayName = name,
                ErrorControl = WindowsServiceErrorControl.Severe
            };

            // act 
            _serviceInstaller.InstallService(config);

            // assert
            var errorControl = ServiceHelper.GetErrorControl(name);

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
                Path = _serviceInstaller.ServicePath,
                DisplayName = name,
                Type = WindowsServiceType.ShareProcess
            };

            // act 
            _serviceInstaller.InstallService(config);

            // assert
            var errorControl = ServiceHelper.GetServiceType(name);

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
                Path = _serviceInstaller.ServicePath,
                DisplayName = name,
                StartMode = WindowsServiceStartMode.Disabled
            };

            // act 
            _serviceInstaller.InstallService(config);

            // assert
            var startMode = ServiceHelper.GetStartMode(name);

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
                Path = _serviceInstaller.ServicePath,
                DisplayName = name,
                StartMode = WindowsServiceStartMode.Manual
            };

            // act 
            _serviceInstaller.InstallService(config);

            // assert
            var startMode = ServiceHelper.GetStartMode(name);

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
                Path = _serviceInstaller.ServicePath,
                DisplayName = name,
                StartMode = WindowsServiceStartMode.Automatic
            };

            // act 
            _serviceInstaller.InstallService(config);

            // assert
            var startMode = ServiceHelper.GetStartMode(name);

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
                Path = _serviceInstaller.ServicePath,
                DisplayName = name,
                InteractWithDesktop = true
            };

            // act 
            _serviceInstaller.InstallService(config);

            // assert
            var type = ServiceHelper.GetServiceType(name);
            var interactiveWithDekstopFlag = 0x100;
            
            type.Should().Be((int) config.Type | interactiveWithDekstopFlag);
        }

        [Test]
        public void Install_Should_Add_ChosenAccount()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);
            var userName = serviceName + "User";
            var userPassword = "test";

            _userInstaller.Install(userName, userPassword);

            var config = new WindowsServiceConfiguration
            {
                Name = serviceName,
                DisplayName = serviceName,
                Path = _serviceInstaller.ServicePath,
                AccountName = userName,
                AccountPassword = userPassword
            };

            // act
            _serviceInstaller.InstallService(config);

            // assert
            var account = ServiceHelper.GetAccount(serviceName);
            account.Should().Be($".\\{userName}");
        }

        [Test]
        public void Install_Should_Throw_When_AccountIsInvalid()
        {
           // arrange 
            var serviceName = _nameGenerator.GetRandomName(Prefix);
            var userName = "fakeUSer";
            var userPassword = "fakePassword";

            var config = new WindowsServiceConfiguration
            {
                Name = serviceName,
                DisplayName = serviceName,
                Path = _serviceInstaller.ServicePath,
                AccountName = userName,
                AccountDomain = userPassword
            };

            // act
            Action act = () => _serviceInstaller.InstallService(config);

            // assert 
            act.ShouldThrow<InvalidOperationException>();
        }
    }
}
