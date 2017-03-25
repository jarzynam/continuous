using System;
using Continuous.User.Users;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.User.Tests
{
    [TestFixture]
    public class UserShellTests
    {
        private IUserShell _shell;
        private Random _random;

        private string RandomUserName => "Test" + _random.Next(0, 5000);

        [SetUp]
        public void Configure()
        {
            _shell = new UserShell();
            _random = new Random();
        }

        [Test]
        public void Can_Create_And_Remeove_User()
        {
            // arrange
            var user = BuildLocalUser();

            // act
            Action addUserDelagate = () => _shell.Create(user);
            Action removeUserDelegate = () => _shell.Remove(user.Name);

            // assert
            addUserDelagate.ShouldNotThrow();
            removeUserDelegate.ShouldNotThrow();
        }

        [Test]
        public void Cant_CreateSameUser_Twice()
        {
            // arrange
            var user = BuildLocalUser();

            // act
            Action act = () => _shell.Create(user);

            // assert
            act.ShouldNotThrow();
            act.ShouldThrow<InvalidOperationException>();
            
            // cleanup
            _shell.Remove(user.Name);
        }

        [Test]
        public void Cant_Remove_NotExistingUser()
        {
            // arrange
            var userName = RandomUserName;

            // act
            Action act = () => _shell.Remove(userName);

            // assert
            act.ShouldThrow<InvalidOperationException>();
        }


        [Test]
        public void Can_Get_User()
        {
            // arrange
            var originalUser = BuildLocalUser();
            _shell.Create(originalUser);

            try
            {
                // act
                var actualUser = _shell.Get(originalUser.Name);

                // assert
                actualUser.Name.Should().Be(originalUser.Name);
                actualUser.Description.Should().Be(originalUser.Description);
                actualUser.Expires.Should().Be(originalUser.Expires);
                actualUser.FullName.Should().Be(originalUser.FullName);
                actualUser.Password.Should().Be(String.Empty);
            }
            finally
            {
                // cleanup
                _shell.Remove(originalUser.Name);
            }

        }

        [Test]
        public void Cant_GetUser_WhenNotExisting()
        {
            // arrange 
            var userName = RandomUserName;

            // act
            Action act = () => _shell.Get(userName);

            // assert
            act.ShouldThrow<InvalidOperationException>();
        }

        private Users.Model.User BuildLocalUser()
        {
            return new Users.Model.User
            {
                Name = RandomUserName,
                FullName = "Test User 1",
                Description = "Delete this user after tests",
                Password = "Test123",
                Expires = new DateTime(2018, 01, 01)
            };
        }
    }
}
