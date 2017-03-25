using System;
using System.Management.Automation;
using System.Security.Principal;
using System.ServiceProcess;
using Continuous.User.Users;
using Continuous.WindowsService.Model;
using Continuous.WindowsService.Model.Enums;
using Continuous.WindowsService.Shell;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.WindowsService.Tests
{
    [TestFixture]
    public class WindowsServiceShellTests
    {
        [SetUp]
        public void Configure()
        {
            _shell = new WindowsServiceShell();
            _userShell = new UserShell();

            _helper = new CompiledServiceTestHelper();
        }

        private CompiledServiceTestHelper _helper;
        private IWindowsServiceShell _shell;
        private IUserShell _userShell;

        [Test]
        public void Can_ChangeUser_InExistingServer_Test()
        {
            // arrange
            var user = new User.Users.Model.User {Name = _helper.RandomServiceName + "User"};
            _userShell.Create(user);

            var serviceName = _helper.RandomServiceName;
            _shell.Install(serviceName, _helper.GetTestServicePath());

            try
            {
                // act
                _shell.ChangeUser(serviceName, user.Name, user.Password);

                // assert
                var service = _shell.Get(serviceName);

                service.AccountDomain.Should().Be(".");
                service.AccountName.Should().Be(user.Name);
            }
            finally
            {
                // cleanup
                _shell.Uninstall(serviceName);
                _userShell.Remove(user.Name);
            }
        }


        [Test]
        public void Can_Check_ServiceStatus_Test()
        {
            // arrange
            var serviceName = _helper.RandomServiceName;
            _shell.Install(serviceName, _helper.GetTestServicePath());

            try
            {
                // act
                var serviceStatus = _shell.GetStatus(serviceName);

                // assert
                serviceStatus.Should().Be(ServiceControllerStatus.Stopped);
            }
            finally
            {
                // cleanup
                _shell.Uninstall(serviceName);
            }
        }

        [Test]
        public void Can_GetService_WhenExists_Test()
        {
            // arrange
            var serviceName = _helper.RandomServiceName;
            var path = _helper.GetTestServicePath();
            _shell.Install(serviceName, path);

            try
            {
                //act
                var service = _shell.Get(serviceName);

                // assert
                service.Name.Should().Be(serviceName);
                service.Description.Should().BeNull();
                service.DisplayName.Should().Be(serviceName);
                service.ProcessId.Should().Be(0);
                service.AccountDomain.Should().Be(null);
                service.AccountName.Should().Be("LocalSystem");
                service.InteractWithDesktop.Should().Be(false);
                service.Path.Should().Be(path);
                service.ExitCode.Should().Be(1077);
                service.ServiceSpecificExitCode.Should().Be(0);

                service.StartMode.Should().Be(WindowsServiceStartMode.Automatic);
                service.State.Should().Be(WindowsServiceState.Stopped);
                service.Status.Should().Be(WindowsServiceStatus.Ok);
                service.Type.Should().Be(WindowsServiceType.OwnProcess);
                service.ErrorControl.Should().Be(WindowsServiceErrorControl.Normal);
            }
            finally
            {
                // cleanup
                _shell.Uninstall(serviceName);
            }
        }

        [Test]
        public void Can_GetService_WhenNotExist()
        {
            // arrange
            var serviceName = "--sdatestas";

            // act
            var service = _shell.Get(serviceName);

            // assert
            service.Should().BeNull();
        }

        [Test]
        public void Can_Install_And_Uninstall_Service_Test()
        {
            // arrange
            new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator)
                .Should()
                .BeTrue();

            var serviceName = _helper.RandomServiceName;
            var servicePath = _helper.GetTestServicePath();

            // act
            Action installAction = () => _shell.Install(serviceName, servicePath);
            Action uninstallAction = () => _shell.Uninstall(serviceName);

            // assert
            installAction.ShouldNotThrow();
            uninstallAction.ShouldNotThrow();
        }

        [Test]
        public void Can_ThrowException_During_Uninstalling_NonExistingService()
        {
            // arrange
            var serviceName = "fakeService";

            // act
            Action uninstallAction = () => _shell.Uninstall(serviceName);

            // assert
            uninstallAction.ShouldThrow<RuntimeException>().WithMessage("fakeService service not found");
        }

        [Test]
        public void Can_Start_RunningService_Test()
        {
            // arrange
            var serviceName = _helper.RandomServiceName;

            _shell.Install(serviceName, _helper.GetTestServicePath());

            try
            {
                // act
                Func<bool> startAction = () => _shell.Start(serviceName);
                Func<ServiceControllerStatus> statusAction = () => _shell.GetStatus(serviceName);

                // assert
                startAction.Invoke().Should().Be(true);
                startAction.Invoke().Should().Be(false);
                statusAction.Invoke().Should().Be(ServiceControllerStatus.Running);
            }
            finally
            {
                // cleanup
                _shell.Uninstall(serviceName);
            }
        }

        [Test]
        public void Can_Start_StoppedService_Test()
        {
            // arrange
            var serviceName = _helper.RandomServiceName;

            _shell.Install(serviceName, _helper.GetTestServicePath());

            try
            {
                var serviceStatus = _shell.GetStatus(serviceName);
                serviceStatus.Should().Be(ServiceControllerStatus.Stopped);

                // act
                Func<bool> startFunct = () => _shell.Start(serviceName);
                Func<ServiceControllerStatus> getStatusFunc = () => _shell.GetStatus(serviceName);

                // assert
                startFunct.Invoke().Should().BeTrue();
                getStatusFunc.Invoke().Should().Be(ServiceControllerStatus.Running);
            }
            finally
            {
                // cleanup
                _shell.Uninstall(serviceName);
            }
        }

        [Test]
        public void Can_Stop_RunningService_Test()
        {
            // arrange
            var serviceName = _helper.RandomServiceName;

            _shell.Install(serviceName, _helper.GetTestServicePath());

            try
            {
                // act
                Func<bool> startAction = () => _shell.Start(serviceName);
                Func<bool> stopAction = () => _shell.Stop(serviceName);
                Func<ServiceControllerStatus> statusAction = () => _shell.GetStatus(serviceName);

                // assert
                startAction.Invoke().Should().BeTrue();
                statusAction.Invoke().Should().Be(ServiceControllerStatus.Running);
                stopAction.Invoke().Should().BeTrue();
                statusAction.Invoke().Should().Be(ServiceControllerStatus.Stopped);

            }
            finally
            {
                // cleanup
                _shell.Uninstall(serviceName);
            }
        }

        [Test]
        public void Can_Stop_StoppedService_Test()
        {
            // arrange
            var serviceName = _helper.RandomServiceName;

            _shell.Install(serviceName, _helper.GetTestServicePath());

            try
            {
                // act
                Func<ServiceControllerStatus> statusAction = () => _shell.GetStatus(serviceName);
                Func<bool> stopAction = () => _shell.Stop(serviceName);

                // assert
                statusAction.Invoke().Should().Be(ServiceControllerStatus.Stopped);
                stopAction.Invoke().Should().BeFalse();
                statusAction.Invoke().Should().Be(ServiceControllerStatus.Stopped);
            }
            finally
            {
                // cleanup
                _shell.Uninstall(serviceName);
            }
        }

        [Test]
        public void Can_Throw_Exception_If_CheckStatus_Before_ConnectingToService_Test()
        {
            // arrange
            var serviceName = "TEST213214";

            // act
            Action act = () => _shell.GetStatus(serviceName);

            // assert
            act.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void Can_Ununinstall_NotExisting_Service_Test()
        {
            // assert
            new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator)
                .Should()
                .BeTrue();

            var serviceName = _helper.RandomServiceName;

            // act
            Action act = () => _shell.Uninstall(serviceName);

            // assert
            act.ShouldThrow<RuntimeException>().WithMessage($"{serviceName} service not found");
        }

        [Test]
        public void Can_Install_Service_With_DefaultConfiguration_Test()
        {
            // assert
            new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator)
               .Should()
               .BeTrue();

            var name = _helper.RandomServiceName;
            var configuration = new WindowsServiceConfiguration()
            {
                Name = name,
                DisplayName = name,
                Path = _helper.GetTestServicePath(),
               
            };
            
            // act 
            Action act = () => _shell.Install(configuration);

            // assert
            act.ShouldNotThrow<InvalidOperationException>();

            try
            {
                var service = _shell.Get(configuration.Name);

                service.Name.Should().Be(configuration.Name);
                service.AccountDomain.Should().Be(configuration.AccountDomain);
                service.AccountName.Should().Be("LocalSystem");
                service.InteractWithDesktop.Should().Be(configuration.InteractWithDesktop);
                service.Path.Should().Be(configuration.Path);
                service.StartMode.Should().Be(configuration.StartMode);
                service.State.Should().Be(WindowsServiceState.Stopped);
                service.Status.Should().Be(WindowsServiceStatus.Ok);
                service.Type.Should().Be(configuration.Type);
                service.Description.Should().Be(null);
                service.ErrorControl.Should().Be(configuration.ErrorControl);
                service.ProcessId.Should().Be(0);
                service.ExitCode.Should().Be(1077);
                service.ServiceSpecificExitCode.Should().Be(0);
                
            }
            finally
            {
                // cleanup
                _shell.Uninstall(configuration.Name);
            }
        }
    }
}