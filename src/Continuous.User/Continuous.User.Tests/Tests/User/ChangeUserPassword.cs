using System;
using Continuous.User.Tests.TestHelpers;
using Continuous.User.Tests.TestHelpers.Installer;
using Continuous.User.Users;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.User.Tests.Tests.User
{
    [TestFixture]
    public class ChangeUserPassword
    {
        private IUserShell _shell;
        private NameGenerator _generator;
        private UserInstaller _installer;

        [SetUp]
        public void SetUp()
        {
            _shell = new UserShell();
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
            Action act = () => _shell.ChangePassword(userName, _generator.RandomName);
            
            // assert
           act.ShouldNotThrow();
        }

        [Test]
        public void ChangePassword_Should_Thorw_When_CantFindUser()
        {
            // arrange
            var userName = _generator.RandomName;

            // act
            Action act = () => _shell.ChangePassword(userName, _generator.RandomName);

            // assert
            act.ShouldThrow<InvalidOperationException>();
        }
    }

}
