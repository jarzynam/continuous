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


        [TestCase("Test User", "Test description", 1)]
        [TestCase("Test User", "Test description", null)]
        [TestCase("Test User", null, 1)]
        [TestCase(null, "Test description", 1)]
        public void Create_Should_Add_NewUser(string fullName, string description, int? accountLifeTimeInDays)
        {
            // arrange
            var user = new LocalUserCreateModel
            {
                Description = description,
                AccountExpires = accountLifeTimeInDays.HasValue ? (DateTime?) DateTime.Now.Date.AddDays(accountLifeTimeInDays.Value): null,
                FullName = fullName,
                Name = _generator.RandomName,
                Password = _generator.RandomName
            };

            // act
            _shell.Create(user);

            _installer.AddAsInstalled(user.Name);

            // assert
            var createdUser = UserHelper.GetUser(user.Name);

            createdUser.Name.Should().Be(user.Name);
            createdUser.Description.Should().Be(user.Description??"");
            createdUser.AccountExpires.Should().Be(user.AccountExpires);
            createdUser.FullName.Should().Be(user.FullName??"");
            createdUser.Password.Should().Be(null);
        }

        [Test]
        public void Create_Throws_When_CreateSameUser_Twice()
        {
            // arrange
            var user = UserHelper.BuildLocalUser(_generator.RandomName);

            // act
            Action act = () => _shell.Create(user);

            _installer.AddAsInstalled(user.Name);

            // assert
            act.Should().NotThrow();
            act.Should().Throw<System.Management.Automation.MethodInvocationException>();
        }
    }

}
