using System;
using Continuous.Management;
using Continuous.WindowsService.Shell;
using Continuous.WindowsService.Tests.TestHelpers;
using Continuous.WindowsService.Tests.TestHelpers.Installer;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.WindowsService.Tests.Tests
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