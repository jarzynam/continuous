using System;
using System.Management.Automation;
using Continuous.User.Tests.TestHelpers;
using Continuous.User.Tests.TestHelpers.Installer;
using Continuous.User.Users;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.User.Tests.Tests.User
{
    [TestFixture]
    public class SetUserFlagsTests
    {
        private const int PasswordNotRequiredFlag = 0x20;
        private const int PasswordCantChangeFlag = 0x40;
        private const int PasswordCantExpireFlag = 0x10000;
        private const int AccountDisabledFlag = 0x2;
       

        private IUserShell _shell;
        private NameGenerator _generator;
        private UserInstaller _installer;

        private bool GetFlag(int flags, int flag)
        {
            return (flags & flag) != 0;
        }

        [SetUp]
        public void SetUp()
        {
            _shell = new UserShell();
            _generator = new NameGenerator("testCUF");
            _installer = new UserInstaller();
        }

        [TearDown]
        public void TearDown()
        {
            _installer.Dispose();
        }

        [Test]
        public void SetPasswordCanChange_ShouldSet_ToFalse()
        {
            // arrange
            var userName = _generator.RandomName;

            _installer.Install(userName, _generator.RandomName);

            // act
            _shell.SetPasswordCanBeChangedByUser(userName, false);
            
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
            _shell.SetPasswordCanBeChangedByUser(userName, true);

            // assert
            var flags = UserHelper.GetUserFlags(userName);

            GetFlag(flags, PasswordCantChangeFlag).Should().BeFalse();
        }

        [Test]
        public void SetPasswordCanChange_Should_Throw_When_CantFindUser()
        {
            // arrange
            var userName = _generator.RandomName;

            // act
            Action act = () => _shell.SetPasswordCanBeChangedByUser(userName, true);

            // assert
            act.Should().Throw<MethodInvocationException>();
        }

        [Test]
        public void SetPasswordCanExpire_ShouldSet_ToFalse()
        {
            // arrange
            var userName = _generator.RandomName;

            _installer.Install(userName, _generator.RandomName);

            // act
            _shell.SetPasswordCanExpire(userName, false);

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
            _shell.SetPasswordCanExpire(userName, true);

            // assert
            var flags = UserHelper.GetUserFlags(userName);

            GetFlag(flags, PasswordCantExpireFlag).Should().BeFalse();
        }

        [Test]
        public void SetPasswordCanExpire_Should_Throw_When_CantFindUser()
        {
            // arrange
            var userName = _generator.RandomName;

            // act
            Action act = () => _shell.SetPasswordCanExpire(userName, true);

            // assert
            act.Should().Throw<MethodInvocationException>();
        }


        [Test]
        public void SetPasswordRequired_ShouldSet_ToFalse()
        {
            // arrange
            var userName = _generator.RandomName;

            _installer.Install(userName, _generator.RandomName);

            // act
            _shell.SetPasswordRequired(userName, false);

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
            _shell.SetPasswordRequired(userName, true);

            // assert
            var flags = UserHelper.GetUserFlags(userName);

            GetFlag(flags, PasswordNotRequiredFlag).Should().BeFalse();
        }

        [Test]
        public void SetPasswordRequired_Should_Throw_When_CantFindUser()
        {
            // arrange
            var userName = _generator.RandomName;

            // act
            Action act = () => _shell.SetPasswordRequired(userName, true);

            // assert
            act.Should().Throw<MethodInvocationException>();
        }

        [Test]
        public void SetAccountDisabled_ShouldSet_ToTrue()
        {
            // arrange
            var userName = _generator.RandomName;

            _installer.Install(userName, _generator.RandomName);

            // act
            _shell.SetAccountDisabled(userName, true);

            // assert
            var flags = UserHelper.GetUserFlags(userName);

            GetFlag(flags, AccountDisabledFlag).Should().BeTrue();
        }

        [Test]
        public void SetAccountDisabled_ShouldSet_ToFalse()
        {
            // arrange
            var userName = _generator.RandomName;

            _installer.Install(userName, _generator.RandomName);
            UserHelper.SetUserFlag(userName, AccountDisabledFlag, true);

            // act
            _shell.SetAccountDisabled(userName, false);

            // assert
            var flags = UserHelper.GetUserFlags(userName);

            GetFlag(flags, AccountDisabledFlag).Should().BeFalse();
        }

        [Test]
        public void SetAccountDisabled_Should_Throw_When_CantFindUser()
        {
            // arrange
            var userName = _generator.RandomName;

            // act
            Action act = () => _shell.SetAccountDisabled(userName, true);

            // assert
            act.Should().Throw<MethodInvocationException>();
        }

       
    }

}
