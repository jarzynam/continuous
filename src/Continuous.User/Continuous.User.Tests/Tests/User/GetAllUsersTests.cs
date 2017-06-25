using System;
using System.Linq;
using System.Runtime.InteropServices;
using Continuous.User.Tests.TestHelpers;
using Continuous.User.Tests.TestHelpers.Installer;
using Continuous.User.Users;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.User.Tests.Tests.User
{
    [TestFixture]
    public class GetAllUsersTests
    {
        [SetUp]
        public void SetUp()
        {
            _shell = new UserShell();
            _generator = new NameGenerator(Prefix);
            _installer = new UserInstaller();
        }

        [TearDown]
        public void TearDown()
        {
            _installer.Dispose();
        }

        private IUserShell _shell;
        private NameGenerator _generator;
        private UserInstaller _installer;
        private const string Prefix = "GAU";

        [Test]
        public void GetAllUsers_NotThrow_Exception()
        {
            // act
            Action act = () => _shell.GetAllUsers();

            // assert
            act.ShouldNotThrow();
        }

        [Test]
        public void GetAllUsers_Fethes_List()
        {
            // act
            var users = _shell.GetAllUsers();

            // assert
            users.Should().NotBeEmpty();
        }

        [Test]
        public void GetAllUsers_Fethes_SpecificUser()
        {
            // arrange
            var userName = _generator.RandomName;

            _installer.Install(userName, _generator.RandomName);

            // act
            var users = _shell.GetAllUsers();

            // assert
            users.Should().Contain(p => p.Name == userName);
        }
    }
}