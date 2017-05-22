using System;
using Continuous.WindowsService.Tests.TestHelpers;
using Continuous.WindowsService.Tests.TestHelpers.Installer;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.WindowsService.Tests.Tests.WindowsServiceInfo
{
    [TestFixture]
    public class ChangeAccountByServiceExtenstionTests
    {
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

        private ServiceInstaller _serviceInstaller;
        private NameGenerator _nameGenerator;
        private UserInstaller _userInstaller;
       
        private const string Prefix = "caTest";

        [Test]
        public void ChangeAccount_Should_ChangeAccount()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);
            var userName = serviceName;
            var userPassword = "test";

            var service = _serviceInstaller.InstallAndGetService(serviceName);
            _userInstaller.Install(userName, userPassword);

            // act
            service.Change()
                .AccountName(userName)
                .AccountPassword(userPassword)
                .Apply();

            // assert
            var account = ServiceHelper.GetAccount(serviceName);

            account.Should().Be(".\\" + userName);
        }

        [Test]
        public void ChangeAccount_Should_Throw_When_User_IsInvalid()
        {
            // arrange
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            var service = _serviceInstaller.InstallAndGetService(serviceName);
           
            // act
            Action act = () => service.Change()
                .AccountName("fakeUser")
                .AccountPassword("fakePassword")
                .Apply();

            // assert
            act.ShouldThrow<InvalidOperationException>();
        }
    }
}