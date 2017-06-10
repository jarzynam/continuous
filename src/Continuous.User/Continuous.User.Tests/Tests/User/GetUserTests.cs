using Continuous.User.Tests.TestHelpers;
using Continuous.User.Tests.TestHelpers.Installer;
using Continuous.User.Users;
using Continuous.User.Users.Model;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.User.Tests.Tests.User
{
    [TestFixture]
    public class GetUserTests
    {
        [SetUp]
        public void SetUp()
        {
            _shell = new UserShell();
            _generator = new NameGenerator("testGU");
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


        [Test]
        public void Get_Fetches_User()
        {
            // arrange
            var originalUser = new UserModel
            {
                Name = _generator.RandomName,
                AccountExpires = null,
                Password = null,
                Description = "",
                FullName = "test user to delete"
            };
            _installer.Install(originalUser);


            // act
            var actualUser = _shell.Get(originalUser.Name);

            // assert
            actualUser.Name.Should().Be(originalUser.Name);
            actualUser.Description.Should().Be(originalUser.Description);
            actualUser.AccountExpires.Should().Be(originalUser.AccountExpires);
            actualUser.FullName.Should().Be(originalUser.FullName);
            actualUser.Password.Should().Be(null);

            actualUser.PasswordLastChange.GetValueOrDefault().Date.Should().Be(UserHelper.GetPasswordLastSet(originalUser.Name).Date);
            actualUser.PasswordMaxBadAttempts.Should().Be(UserHelper.GetPasswordMaxBadAttempts(originalUser.Name));
            actualUser.PasswordBadAttemptsInterval.Should().Be(UserHelper.GetPasswordBadAttemptsInterval(originalUser.Name));
            actualUser.PasswordMustChangeOnNextLogon.Should().Be(false);
            actualUser.PasswordExpires.GetValueOrDefault().Date.Should().Be(UserHelper.GetPasswordExpirationDate(originalUser.Name).Date);
            actualUser.PasswordMinLength.Should().Be(UserHelper.GetPasswordMinLength(originalUser.Name));
            actualUser.PasswordCantChange.Should().Be(false);
            actualUser.PasswordNotRequired.Should().Be(false);
        }

        [Test]
        public void Get_ReturnNull_When_User_NotExisting()
        {
            // arrange 
            var userName = _generator.RandomName;

            // act
            var user = _shell.Get(userName);

            // assert
            user.Should().BeNull();
        }
    }
}