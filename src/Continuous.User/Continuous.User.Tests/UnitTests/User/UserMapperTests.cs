using System;
using Continuous.User.Users;
using Continuous.User.Users.Model;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.User.Tests.UnitTests
{
    [TestFixture]
    public class UserMapperTests
    {
        private UserMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = new UserMapper();
        }

        [Test]
        public void MapToUserCreateModel_Maps_WhenOk()
        {
            // arrange 
            var model = new UserModel
            {
                Name = "test name",
                Description = "test description",
                Password = "test password",
                FullName = "test name",
                AccountExpires = null
            };

            // act
            var info = _mapper.MapToUserCreateModel(model);

            // assert
            info.Name.Should().Be(model.Name);
            info.AccountExpires.Should().Be(model.AccountExpires);
            info.Description.Should().Be(model.Description);
            info.FullName.Should().Be(model.FullName);
            info.Password.Should().Be(model.Password);
        }


        [Test]
        public void MapToUserModel_Maps_WhenOk()
        {
            // arrange
            var info = new LocalUserInfo
            {
                Name = "test name",
                Description = "test description",
                FullName = "full name",
                AccountExpires = new DateTime(2000, 1, 1),
                PasswordMaxBadAttempts = 1,
                PasswordMinLength = 2,
                PasswordRequired = true,
                PasswordExpires = new DateTime(2000, 1, 2),
                PasswordBadAttemptsInterval = TimeSpan.Zero,
                PasswordCanBeChangedByUser = true,
                PasswordLastChange = new DateTime(2000, 1, 3),
                PasswordMustBeChangedAtNextLogon = true
            };

            // act
            var model = _mapper.MapToUserModel(info);

            // assert
            model.Name.Should().Be(info.Name);
            model.AccountExpires.Should().Be(info.AccountExpires);
            model.Description.Should().Be(info.Description);
            model.FullName.Should().Be(info.FullName);
            model.Password.Should().Be(null);
            model.PasswordBadAttemptsInterval.Should().Be(info.PasswordBadAttemptsInterval);
            model.PasswordCanBeChangedByUser.Should().Be(info.PasswordCanBeChangedByUser);
            model.PasswordExpires.Should().Be(info.PasswordExpires);
            model.PasswordLastChange.Should().Be(info.PasswordLastChange);
            model.PasswordMaxBadAttempts.Should().Be(info.PasswordMaxBadAttempts);
            model.PasswordMinLength.Should().Be(info.PasswordMinLength);
            model.PasswordMustBeChangedAtNextLogon.Should().Be(info.PasswordMustBeChangedAtNextLogon);
            model.PasswordRequired.Should().Be(info.PasswordRequired);
        }

        [Test]
        public void MapToUserModel_Maps_WhenNull()
        {
            // act
            var model = _mapper.MapToUserModel(null);

            // assert
            model.Should().Be(null);
        }
    }
}
