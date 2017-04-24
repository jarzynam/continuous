using System;
using Continuous.Management;
using Continuous.Test.WindowsService.Logic;
using Continuous.Test.WindowsService.Logic.Installer;
using Continuous.Test.WindowsService.TestHelpers;
using Continuous.WindowsService;
using Continuous.WindowsService.Shell;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.Test.WindowsService.Tests
{
    [TestFixture]
    public class ChangeAccountTests
    {
        [SetUp]
        public void SetUp()
        {
            _shell = (new ContinuousManagementFactory()).WindowsServiceShell();
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

        private ServiceInstaller _serviceInstaller;
        private NameGenerator _nameGenerator;
        private UserInstaller _userInstaller;
        private IWindowsServiceShell _shell;

        private const string Prefix = "caTest";

        [Test]
        public void ChangeAccount_Should_ChangeAccount()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);
            var userName = serviceName;
            var userPassword = "test";

            _serviceInstaller.InstallService(serviceName);
            _userInstaller.Install(userName, userPassword);

            // act
            _shell.ChangeAccount(serviceName, userName, userPassword);

            // assert
            var account = ServiceHelper.GetAccount(serviceName);

            account.Should().Be(".\\" + userName);
        }

        [Test]
        public void ChangeAccount_Should_Throw_When_User_IsInvalid()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);
           
            _serviceInstaller.InstallService(serviceName);
            
            // act
            Action act =  () => _shell.ChangeAccount(serviceName, "fakeUser", "falePassword");

            // assert
            act.ShouldThrow<InvalidOperationException>();
        }
    }
}