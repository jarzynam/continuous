using System;
using Continuous.User.Tests.TestHelpers;
using Continuous.User.Tests.TestHelpers.Installer;
using Continuous.User.Users;
using Continuous.User.Users.Model;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.User.Tests.Tests.User
{
    [TestFixture]
    public class ChangeUserPasswordByExtensionTests
    {
        private NameGenerator _generator;
        private UserInstaller _installer;

        [SetUp]
        public void SetUp()
        {
            new UserShell();
            _generator = new NameGenerator("testCUP");
            _installer = new UserInstaller();
        }

        [TearDown]
        public void TearDown()
        {
            _installer.Dispose();
        }

        [Test]
        public void ChangePassword_Should_NotThrow()
        {
            // arrange
            var userName = _generator.RandomName;

            _installer.Install(userName, _generator.RandomName);

            // act
            Action act = () => new LocalUserInfo(userName)
                .Change()
                .Password(_generator.RandomPassword)
                .Apply();

            // assert
            act.Should().NotThrow();
        }
    

    [Test]
        public void ChangePassword_Should_Thorw_When_CantFindUser()
        {
            // arrange
            var userName = _generator.RandomName;

            // act
            Action act = () => new LocalUserInfo(userName)
                .Change()
                .Password(_generator.RandomPassword)
                .Apply();

            // assert
            act.Should().Throw<InvalidOperationException>();
        }
    }

}
