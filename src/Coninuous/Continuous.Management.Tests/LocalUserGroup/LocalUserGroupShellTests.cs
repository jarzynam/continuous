using System;
using System.Collections.Generic;
using Continuous.Management.LocalUser;
using Continuous.Management.LocalUserGroup;
using NUnit.Framework;

namespace Continuous.Management.Tests.LocalUserGroup
{
    [TestFixture]
    public class LocalUserGroupShellTests
    {
        private ILocalUserGroupShell _shell;
        private LocalUserShell _userShell;
        private Random _random;

        private const string DefaultDescription = "test group to delete";
        private string RandomName => "Test" + _random.Next(0, 5000);

        [SetUp]
        public void Configure()
        {
            _shell = new LocalUserGroupShell();
            _userShell = new LocalUserShell();
            _random = new Random();
        }

        [Test]
        public void Can_Create_And_Remeove_Group()
        {
            string name = RandomName;

            TestDelegate addDelegate = () => _shell.Create(name, DefaultDescription);
            TestDelegate removeDelegate = () => _shell.Remove(name);

            Assert.DoesNotThrow(addDelegate);
            Assert.DoesNotThrow(removeDelegate);
        }

        [Test]
        public void Cant_CreateSameGroup_Twice()
        {
            var name = RandomName;

            TestDelegate act = () => _shell.Create(name, DefaultDescription);

            Assert.DoesNotThrow(act);
            Assert.Throws<InvalidOperationException>(act);

            _shell.Remove(name);
        }

        [Test]
        public void Cant_Remove_NotExistingGroup()
        {
            var name = RandomName;

            TestDelegate act = () => _shell.Remove(name);

            Assert.Throws<InvalidOperationException>(act);
        }


        [Test]
        public void Can_Get_Group()
        {
            var name = CreateGroup();

            try
            {
                var actualGroup = _shell.Get(name);

                Assert.AreEqual(name, actualGroup.Name);
                Assert.AreEqual(DefaultDescription, actualGroup.Description);
            }
            finally
            {
                _shell.Remove(name);
            }
        }

        [Test]
        public void Cant_GetGroup_WhenNotExisting()
        {
            var group = RandomName;

            TestDelegate act = () => _shell.Get(group);

            Assert.Throws<InvalidOperationException>(act);
        }

        [Test]
        public void Can_Assign_User_ToGroup()
        {
            var user = new Management.LocalUser.Model.LocalUser
            {
                Name = RandomName + "User",
                Description = "test to delete"
            };
            _userShell.CreateUser(user);
            var groupName = CreateGroup();

            try
            {
               _shell.AssignUsers(groupName, new List<string>{user.Name});

                var group = _shell.Get(groupName);
                Assert.IsTrue(group.Members.Contains(user.Name));
            }
            finally
            {
                _shell.Remove(groupName);
                _userShell.RemoveUser(user.Name);
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
