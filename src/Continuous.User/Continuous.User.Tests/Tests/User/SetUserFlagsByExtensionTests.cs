using System;
using Continuous.User.Tests.TestHelpers;
using Continuous.User.Tests.TestHelpers.Installer;
using Continuous.User.Users.Model;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.User.Tests.Tests.User
{
    [TestFixture]
    public class SetUserFlagsByExtensionTests
    {
        [SetUp]
        public void SetUp()
        {
            _generator = new NameGenerator("testCUF");
            _installer = new UserInstaller();
        }

        [TearDown]
        public void TearDown()
        {
            _installer.Dispose();
        }

        private const int PasswordNotRequiredFlag = 0x20;
        private const int PasswordCantChangeFlag = 0x40;
        private const int PasswordCantExpireFlag = 0x10000;
        private const int AccountDisabledFlag = 0x2;

        private NameGenerator _generator;
        private UserInstaller _installer;

        private bool GetFlag(int flags, int flag)
        {
            return (flags & flag) != 0;
        }

        [Test]
        public void SetAccountDisabled_Should_Throw_When_CantFindUser()
        {
            // arrange
            var userName = _generator.RandomName;

            // act
            Action act = () => new LocalUserInfo(userName)
                .Change()
                .AccountDisabled(true)
                .Apply();

            // assert
            act.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void SetAccountDisabled_ShouldSet_ToFalse()
        {
            // arrange
            var userName = _generator.RandomName;

            _installer.Install(userName, _generator.RandomName);
            UserHelper.SetUserFlag(userName, AccountDisabledFlag, true);

            // act
            new LocalUserInfo(userName)
                .Change()
                .AccountDisabled(false)
                .Apply();

            // assert
            var flags = UserHelper.GetUserFlags(userName);

            GetFlag(flags, AccountDisabledFlag).Should().BeFalse();
        }

        [Test]
        public void SetAccountDisabled_ShouldSet_ToTrue()
        {
            // arrange
            var userName = _generator.RandomName;

            _installer.Install(userName, _generator.RandomName);

            // act
            new LocalUserInfo(userName)
                .Change()
                .AccountDisabled(true)
                .Apply();

            // assert
            var flags = UserHelper.GetUserFlags(userName);

            GetFlag(flags, AccountDisabledFlag).Should().BeTrue();
        }

        [Test]
        public void SetPasswordCanChange_Should_Throw_When_CantFindUser()
        {
            // arrange
            var userName = _generator.RandomName;

            // act
            Action act = () => new LocalUserInfo(userName)
                .Change()
                .PasswordCanBeChangedByUser(true)
                .Apply();

            // assert
            act.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void SetPasswordCanChange_ShouldSet_ToFalse()
        {
            // arrange
            var userName = _generator.RandomName;

            _installer.Install(userName, _generator.RandomName);

            // act
            new LocalUserInfo(userName).Change()
                .PasswordCanBeChangedByUser(false)
                .Apply();

            // assert
            var flags = UserHelper.GetUserFlags(userName);

            GetFlag(flags, PasswordCantChangeFlag).Should().BeTrue();
        }

        [Test]
        public void SetPasswordCanChange_ShouldSet_ToTrue()
        {
            // arrange
            var userName = _generator.RandomName;

            _installer.Install(userName, _generator.RandomName);
            UserHelper.SetUserFlag(userName, PasswordCantChangeFlag, true);

            // act
            new LocalUserInfo(userName).Change()
                .PasswordCanBeChangedByUser(true)
                .Apply();

            // assert
            var flags = UserHelper.GetUserFlags(userName);

            GetFlag(flags, PasswordCantChangeFlag).Should().BeFalse();
        }

        [Test]
        public void SetPasswordCanExpire_Should_Throw_When_CantFindUser()
        {
            // arrange
            var userName = _generator.RandomName;

            // act
            Action act = () => new LocalUserInfo(userName)
                .Change()
                .PasswordCanExpire(true)
                .Apply();

            // assert
            act.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void SetPasswordCanExpire_ShouldSet_ToFalse()
        {
            // arrange
            var userName = _generator.RandomName;

            _installer.Install(userName, _generator.RandomName);

            // act
            new LocalUserInfo(userName).Change()
                .PasswordCanExpire(false)
                .Apply();

            // assert
            var flags = UserHelper.GetUserFlags(userName);

            GetFlag(flags, PasswordCantExpireFlag).Should().BeTrue();
        }

        [Test]
        public void SetPasswordCanExpire_ShouldSet_ToTrue()
        {
            // arrange
            var userName = _generator.RandomName;

            _installer.Install(userName, _generator.RandomName);
            UserHelper.SetUserFlag(userName, PasswordCantExpireFlag, true);

            // act
            new LocalUserInfo(userName)
                .Change()
                .PasswordCanExpire(true)
                .Apply();

            // assert
            var flags = UserHelper.GetUserFlags(userName);

            GetFlag(flags, PasswordCantExpireFlag).Should().BeFalse();
        }

        [Test]
        public void SetPasswordRequired_Should_Throw_When_CantFindUser()
        {
            // arrange
            var userName = _generator.RandomName;

            // act
            Action act = () => new LocalUserInfo(userName)
                .Change()
                .PasswordRequired(true)
                .Apply();

            // assert
            act.ShouldThrow<InvalidOperationException>();
        }


        [Test]
        public void SetPasswordRequired_ShouldSet_ToFalse()
        {
            // arrange
            var userName = _generator.RandomName;

            _installer.Install(userName, _generator.RandomName);

            // act
            new LocalUserInfo(userName)
                .Change()
                .PasswordRequired(false)
                .Apply();

            // assert
            var flags = UserHelper.GetUserFlags(userName);

            GetFlag(flags, PasswordNotRequiredFlag).Should().BeTrue();
        }

        [Test]
        public void SetPasswordRequired_ShouldSet_ToTrue()
        {
            // arrange
            var userName = _generator.RandomName;

            _installer.Install(userName, _generator.RandomName);
            UserHelper.SetUserFlag(userName, PasswordNotRequiredFlag, true);

            // act
            new LocalUserInfo(userName)
                .Change()
                .PasswordRequired(true)
                .Apply();

            // assert
            var flags = UserHelper.GetUserFlags(userName);

            GetFlag(flags, PasswordNotRequiredFlag).Should().BeFalse();
        }
    }
}