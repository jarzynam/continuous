using System;
using System.Collections.Generic;
using Continuous.User.LocalUserGroups;
using Continuous.User.Users;
using Continuous.User.Users.Model;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.User.Tests
{
    [TestFixture]
    public class LocalUserGroupShellTests
    {
        private ILocalUserGroupShell _shell;
        private IUserShell _userShell;
        private Random _random;

        private const string DefaultDescription = "test group to delete";
        private string RandomName => "Test" + _random.Next(0, 5000);

        [SetUp]
        public void Configure()
        {
            _shell = new LocalUserGroupShell();
            _userShell = new UserShell();
            _random = new Random();
        }

        [Test]
        public void Can_Create_And_Remeove_Group()
        {
            // arrange
            string name = RandomName;

            // act
            Action addAction = () => _shell.Create(name, DefaultDescription);
            Action removeAction = () => _shell.Remove(name);

            // assert
            addAction.ShouldNotThrow();
            removeAction.ShouldNotThrow();
        }

        [Test]
        public void Cant_CreateSameGroup_Twice()
        {
            // arrange
            var name = RandomName;

            // act
            Action act = () => _shell.Create(name, DefaultDescription);

            // assert
            act.ShouldNotThrow();
            act.ShouldThrow<InvalidOperationException>();
            
            // cleanup
            _shell.Remove(name);
        }

        [Test]
        public void Cant_Remove_NotExistingGroup()
        {
            // arrange
            var name = RandomName;

            // action
            Action act = () => _shell.Remove(name);

            // assert
            act.ShouldThrow<InvalidOperationException>();
        }


        [Test]
        public void Can_Get_Group()
        {
            // arrange
            var name = CreateGroup();

            try
            {
                // act
                var actualGroup = _shell.Get(name);

                // assert
                actualGroup.Name.Should().Be(name);
                actualGroup.Description.Should().Be(DefaultDescription);
            }
            finally
            {
                // cleanup
                _shell.Remove(name);
            }
        }

        [Test]
        public void Cant_GetGroup_WhenNotExisting()
        {
            // arrange
            var group = RandomName;

            // act
            Action act = () => _shell.Get(group);

            // assert
            act.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void Can_Assign_User_ToGroup()
        {
            // arrange
            var user = new UserModel
            {
                Name = RandomName + "User",
                Description = "test to delete"
            };
            _userShell.Create(user);
            var groupName = CreateGroup();

            try
            {
                // act
               _shell.AssignUsers(groupName, new List<string>{user.Name});

                // assert
                var group = _shell.Get(groupName);
                group.Members.Should().Contain(user.Name);
            }
            finally
            {
                // cleanup
                _shell.Remove(groupName);
                _userShell.Remove(user.Name);
            }
        }

        [Test]
        public void Can_ThrowException_When_Assign_EmptyUserList_ToGroup()
        {

            // arrange
            var groupName = CreateGroup();

            try
            {
                // act
                Action act  = () => _shell.AssignUsers(groupName, new List<string>());

                // assert
                act.ShouldThrow<InvalidOperationException>();
            }
            finally
            {
                // cleanup
                _shell.Remove(groupName);
            }
        }

        [Test]
        public void Can_ThorwException_When_TryAddUser_ToNotExistingGroup()
        {
            // arrange
            var user = new UserModel
            {
                Name = RandomName + "User",
                Description = "Test user to remove"
            };
            _userShell.Create(user);

            var groupName = "fake group";

            try
            {
                // act
                Action act = () => _shell.AssignUsers(groupName, new List<string> { user.Name });

                // assert
                act.ShouldThrow<InvalidOperationException>();
            }
            finally
            {
                _userShell.Remove(user.Name);
            }
        }

        [Test]
        public void Can_Remove_User_FromGroup()
        {
            // arrange
            var user = new UserModel
            {
                Name = RandomName + "User",
                Description = "test user to delete"
            };

            var groupName = CreateGroup();

            _userShell.Create(user);
            
            try
            {
                _shell.AssignUsers(groupName, new List<string>{user.Name});

                var group = _shell.Get(groupName);

                group.Members.Should().Contain(p => p == user.Name);

                // act 
                _shell.RemoveUsers(groupName, new List<string>{user.Name});

                // assert
                group = _shell.Get(groupName);

                group.Members.Should().BeEmpty();

            }
            finally
            {
                // cleanup 
                _shell.Remove(groupName);
                _userShell.Remove(user.Name);
            }
        }

        [Test]
        public void Can_ThrowException_When_Try_RemoveEmptyList_FromLocalGroup()
        {
            // arrange
            var users = new List<string>();
            var groupName = CreateGroup();

            try
            {
                // act
                Action act = () => _shell.RemoveUsers(groupName, users);

                // assert
                act.ShouldThrow<InvalidOperationException>();
            }
            finally
            {
                //cleanup
                _shell.Remove(groupName);
            }
        }

        [Test]
        public void Can_ThorwException_When_AddUser_ToNotExistingGroup()
        {
            // arrange
            var user = new UserModel
            {
                Name = RandomName + "User",
                Description = "Test user to remove"
            };
            _userShell.Create(user);

            var groupName = "fake group";

            try
            {
                // act
                Action act = () => _shell.RemoveUsers(groupName, new List<string> {user.Name});

                // assert
                act.ShouldThrow<InvalidOperationException>();
            }
            finally
            {
                _userShell.Remove(user.Name);
            }
        }

        private string CreateGroup()
        {
            var name = RandomName;

            _shell.Create(name, DefaultDescription);

            return name;
        }
    }
}
