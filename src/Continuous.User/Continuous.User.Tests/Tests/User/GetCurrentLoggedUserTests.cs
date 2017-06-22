using Continuous.User.Tests.TestHelpers;
using Continuous.User.Users;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.User.Tests.Tests.User
{
    [TestFixture]
    public class GetCurrentLoggedUserTest
    {
        [SetUp]
        public void SetUp()
        {
            _shell = new UserShell();
        }

        private IUserShell _shell;


        [Test]
        public void GetCurrentLoggedInUser_Fetches_User()
        {
            // act
            var actualUser = _shell.GetCurrentLoggedInUser();

            // assert
            var username = UserHelper.GetCurrentLoggedUserName();
            var originalUser = UserHelper.GetUser(username);

            actualUser.Name.ToLower().Should().Be(username.ToLower());
            actualUser.Description.Should().Be(originalUser.Description);
            actualUser.AccountExpires.Should().Be(originalUser.AccountExpires);
            actualUser.FullName.Should().Be(originalUser.FullName);

            actualUser.PasswordLastChange.GetValueOrDefault()
                .Date.Should()
                .Be(UserHelper.GetPasswordLastSet(originalUser.Name).Date);
            actualUser.PasswordMaxBadAttempts.Should().Be(UserHelper.GetPasswordMaxBadAttempts(originalUser.Name));
            actualUser.PasswordBadAttemptsInterval.Should()
                .Be(UserHelper.GetPasswordBadAttemptsInterval(originalUser.Name));
            actualUser.PasswordMustBeChangedAtNextLogon.Should().Be(false);
            actualUser.PasswordExpires.GetValueOrDefault()
                .Date.Should()
                .Be(UserHelper.GetPasswordExpirationDate(originalUser.Name).GetValueOrDefault().Date);
            actualUser.PasswordMinLength.Should().Be(UserHelper.GetPasswordMinLength(originalUser.Name));
            actualUser.PasswordCanBeChangedByUser.Should().Be(true);
            actualUser.PasswordRequired.Should().Be(true);
            actualUser.AutoUnlockInterval.Should().Be(UserHelper.GetAutoUnlockInterval(originalUser.Name));
            actualUser.LastLogon.Should().Be(UserHelper.GetLastLogon(originalUser.Name));
        }
    }
}