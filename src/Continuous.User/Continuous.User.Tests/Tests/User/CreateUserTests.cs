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
    public class CreateUserTests
    {
        private IUserShell _shell;
        private NameGenerator _generator;
        private UserInstaller _installer;

        [SetUp]
        public void SetUp()
        {
            _shell = new UserShell();
            _generator = new NameGenerator("testCU");
            _installer = new UserInstaller();
        }

        [TearDown]
        public void TearDown()
        {
            _installer.Dispose();
        }

        [Test]
        public void Create_Should_Add_NewUser()
        {
            // arrange
            var user = new UserModel
            {
                Description = "Test",
                Expires = DateTime.Today.AddDays(1),
                FullName = "Test User",
                Name = _generator.RandomName,
                Password = _generator.RandomName
            };

            // act
            _shell.Create(user);

            _installer.AddAsInstalled(user.Name);

            // assert
            var createdUser = UserHelper.GetUser(user.Name);

            createdUser.Name.Should().Be(user.Name);
            createdUser.Description.Should().Be(user.Description);
            createdUser.Expires.Should().Be(user.Expires);
            createdUser.FullName.Should().Be(user.FullName);
            createdUser.Password.Should().Be(null);
        }

        [Test]
        public void Create_Throws_When_CreateSameUser_Twice()
        {
            // arrange
            var user = UserHelper.BuildLocalUser(_generator.RandomName);

            // act
            Action act = () => _shell.Create(user);

            _installer.AddAsInstalled(_generator.RandomName);

            // assert
            act.ShouldNotThrow();
            act.ShouldThrow<InvalidOperationException>();
        }
    }

}
