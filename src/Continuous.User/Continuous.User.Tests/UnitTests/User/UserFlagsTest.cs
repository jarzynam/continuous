using FluentAssertions;
using NUnit.Framework;

namespace Continuous.User.Tests.UnitTests
{
    [TestFixture]
    public class UserFlagsTest
    {
        //all user flags: 
        //https://msdn.microsoft.com/en-us/library/aa772300%28v=vs.85%29.aspx
        private const int PasswordNotRequiredFlag = 0x20;
        private const int PasswordDontExipredFlag = 0x10000;
        private const int AccountDisabledFlag = 0x2;
        private const int AccountLockedFlag = 0x10;

        private const int NegetiveBase = 0x10000000;
        private const int PositiveBase = 0x1FFFFFFF;


        private int SetBit(int position, bool value)
        {
            return value
                ? NegetiveBase | position
                : PositiveBase & ~position;
        }

        [Test]
        public void PasswordRequired_Should_BeFalse()
        {
            // arrange
            var flags = SetBit(PasswordNotRequiredFlag, true);

            var userFlags = new Users.UserFlags(flags);

            // act
            var result = userFlags.PasswordRequired;

            // assert
            result.Should().Be(false);
        }

        [Test]
        public void PasswordRequired_Should_BeTrue()
        {
            // arrange
            var flags = SetBit(PasswordNotRequiredFlag, false);

            var userFlags = new Users.UserFlags(flags);

            // act
            var result = userFlags.PasswordRequired;

            // assert
            result.Should().Be(true);
        }

        [Test]
        public void PasswordCanExpire_Should_BeTrue()
        {
            // arrange
            var flags = SetBit(PasswordDontExipredFlag, false);

            var userFlags = new Users.UserFlags(flags);

            // act
            var result = userFlags.PasswordCanExpire;

            // assert
            result.Should().Be(true);
        }

        [Test]
        public void PasswordCanExpire_Should_BeFalse()
        {
            // arrange
            var flags = SetBit(PasswordDontExipredFlag, true);

            var userFlags = new Users.UserFlags(flags);

            // act
            var result = userFlags.PasswordCanExpire;

            // assert
            result.Should().Be(false);
        }

        [Test]
        public void AccountDisabled_Should_BeTrue()
        {
            // arrange
            var flags = SetBit(AccountDisabledFlag, true);

            var userFlags = new Users.UserFlags(flags);

            // act
            var result = userFlags.AccountDisabled;

            // assert
            result.Should().Be(true);
        }

        [Test]
        public void AccountDisabled_Should_BeFalse()
        {
            // arrange
            var flags = SetBit(AccountDisabledFlag, false);

            var userFlags = new Users.UserFlags(flags);

            // act
            var result = userFlags.AccountDisabled;

            // assert
            result.Should().Be(false);
        }


        [Test]
        public void AccountLockedOut_Should_BeTrue()
        {
            // arrange
            var flags = SetBit(AccountLockedFlag, true);

            var userFlags = new Users.UserFlags(flags);

            // act
            var result = userFlags.AccountLocked;

            // assert
            result.Should().Be(true);
        }

        [Test]
        public void AccountLockedOut_Should_BeFalse()
        {
            // arrange
            var flags = SetBit(AccountLockedFlag, false);

            var userFlags = new Users.UserFlags(flags);

            // act
            var result = userFlags.AccountLocked;

            // assert
            result.Should().Be(false);
        }
    }
}