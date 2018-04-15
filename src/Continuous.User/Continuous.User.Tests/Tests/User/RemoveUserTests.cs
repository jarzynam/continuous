using System;
using Continuous.User.Tests.TestHelpers;
using Continuous.User.Tests.TestHelpers.Installer;
using Continuous.User.Users;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.User.Tests.Tests.User
{
    [TestFixture]
    public class RemoveUserTests
    {
        private IUserShell _shell;
        private NameGenerator _generator;
        private UserInstaller _installer;

        [SetUp]
        public void SetUp()
        {
            _shell = new UserShell();
            _generator = new NameGenerator("testRU");
            _installer = new UserInstaller();
        }

        [TearDown]
        public void TearDown()
        {
            _installer.Dispose();
        }

        [Test]
        public void Remove_Should_Delete_NewUser()
        {
            // arrange
            var userName = _generator.RandomName;

            _installer.Install(userName, _generator.RandomName);

            // act
            _shell.Remove(userName);
            
            // assert
            var user = UserHelper.GetUser(userName);

            user.Should().BeNull();
        }
        [Test]
        public void Remove_Throws_When_User_NotExist()
        {
            // arrange
            var userName = _generator.RandomName;

            // act
            Action act = () => _shell.Remove(userName);

            // assert
            act.Should().Throw<System.Management.Automation.MethodInvocationException>();
        }

    }

}
