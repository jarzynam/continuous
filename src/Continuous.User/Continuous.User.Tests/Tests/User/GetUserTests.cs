﻿using Continuous.User.Tests.TestHelpers;
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
            var originalUser = new LocalUserCreateModel()
            {
                Name = _generator.RandomName,
                AccountExpires = null,
                Password = null,
                Description = "",
                FullName = "test user to delete"
            };
            _installer.Install(originalUser);


            // act
            var actualUser = _shell.GetLocalUser(originalUser.Name);

            // assert
            actualUser.Name.Should().Be(originalUser.Name);
            actualUser.Description.Should().Be(originalUser.Description);
            actualUser.AccountExpires.Should().Be(originalUser.AccountExpires);
            actualUser.FullName.Should().Be(originalUser.FullName);
         
            actualUser.PasswordLastChange.GetValueOrDefault().Date.Should().Be(UserHelper.GetPasswordLastSet(originalUser.Name).Date);
            actualUser.PasswordMaxBadAttempts.Should().Be(UserHelper.GetPasswordMaxBadAttempts(originalUser.Name));
            actualUser.PasswordBadAttemptsInterval.Should().Be(UserHelper.GetPasswordBadAttemptsInterval(originalUser.Name));
            actualUser.PasswordMustBeChangedAtNextLogon.Should().Be(false);
            actualUser.PasswordExpires.GetValueOrDefault().Date.Should().Be(UserHelper.GetPasswordExpirationDate(originalUser.Name).Date);
            actualUser.PasswordMinLength.Should().Be(UserHelper.GetPasswordMinLength(originalUser.Name));
            actualUser.PasswordCanBeChangedByUser.Should().Be(true);
            actualUser.PasswordRequired.Should().Be(true);
        }

        [Test]
        public void Get_WhenPasswordExpired_ReturnsProperFlag()
        {
            // arrange
            var userName = _generator.RandomName;
            
            _installer.Install(userName, _generator.RandomName);

            UserHelper.SetPasswordExipred(userName, true);
            UserHelper.GetPasswordExpired(userName).Should().BeTrue();

            // act
            var user = _shell.GetLocalUser(userName);

            // assert
            user.PasswordMustBeChangedAtNextLogon.Should().BeTrue();
        }

        [Test]
        public void Get_WhenPasswordSetFromExpiredToNot_ReturnsProperFlag()
        {
            // arrange
            var userName = _generator.RandomName;

            _installer.Install(userName, _generator.RandomName);

            UserHelper.SetPasswordExipred(userName, true);
            UserHelper.SetPasswordExipred(userName, false);
            UserHelper.GetPasswordExpired(userName).Should().BeFalse();

            // act
            var user = _shell.GetLocalUser(userName);

            // assert
            user.PasswordMustBeChangedAtNextLogon.Should().BeFalse();
        }

        [Test]
        public void Get_WhenPasswordNeverExpires_ReturnsNullDateTime()
        {
            // arrange
            var userName = _generator.RandomName;

            _installer.Install(userName, _generator.RandomName);

            UserHelper.SetUserFlag(userName, 0x10000, true);
            
            // act
            var user = _shell.GetLocalUser(userName);

            // assert
            user.PasswordExpires.Should().BeNull();
        }

        [Test]
        public void Get_WhenPassordCantChange_ReturnsProperFlag()
        {

            // arrange
            var userName = _generator.RandomName;

            _installer.Install(userName, _generator.RandomName);

            UserHelper.SetUserFlag(userName, 0x40, true);

            // act
            var user = _shell.GetLocalUser(userName);

            // assert
            user.PasswordCanBeChangedByUser.Should().BeFalse();
        }

        [Test]
        public void Get_WhenPasswordNotRequired_ReturnsProperFlag()
        {
            // arrange
            var userName = _generator.RandomName;

            _installer.Install(userName, _generator.RandomName);

            UserHelper.SetUserFlag(userName, 0x20, true);

            // act
            var user = _shell.GetLocalUser(userName);
            
            // assert
            user.PasswordRequired.Should().BeFalse();
        }

        [Test]
        public void Get_ReturnNull_When_User_NotExisting()
        {
            // arrange 
            var userName = _generator.RandomName;

            // act
            var user = _shell.GetLocalUser(userName);

            // assert
            user.Should().BeNull();
        }
    }
}