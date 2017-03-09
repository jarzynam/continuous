using System;
using Continuous.Management.LocalUser;
using NUnit.Framework;

namespace Continuous.Management.Tests.LocalUser
{
    [TestFixture]
    public class LocalUserShellTests
    {
        private ILocalUserShell _shell;
        private Random _random;

        private string RandomUserName => "Test" + _random.Next(0, 5000);

        [SetUp]
        public void Configure()
        {
            _shell = new LocalUserShell();
            _random = new Random();
        }

        [Test]
        public void Can_Create_And_Remeove_User()
        {
            var user = BuildLocalUser();

            TestDelegate addUserDelagate = () => _shell.CreateUser(user);
            TestDelegate removeUserDelegate = () => _shell.RemoveUser(user.Name);

            Assert.DoesNotThrow(addUserDelagate);
            Assert.DoesNotThrow(removeUserDelegate);
        }

        [Test]
        public void Cant_CreateSameUser_Twice()
        {
            var user = BuildLocalUser();

            TestDelegate act = () => _shell.CreateUser(user);

            Assert.DoesNotThrow(act);
            Assert.Throws<InvalidOperationException>(act);

            _shell.RemoveUser(user.Name);
        }

        [Test]
        public void Cant_Remove_NotExistingUser()
        {
            var userName = RandomUserName;

            TestDelegate act = () => _shell.RemoveUser(userName);

            Assert.Throws<InvalidOperationException>(act);
        }


        [Test]
        public void Can_Get_User()
        {
            var originalUser = BuildLocalUser();

            _shell.CreateUser(originalUser);

            try
            {
                var actualUser = _shell.GetUser(originalUser.Name);

                Assert.AreEqual(originalUser.Name, actualUser.Name);
                Assert.AreEqual(originalUser.Description, actualUser.Description);
                Assert.AreEqual(originalUser.Expires, actualUser.Expires);
                Assert.AreEqual(originalUser.FullName, actualUser.FullName);
                Assert.AreEqual(String.Empty, actualUser.Password);
            }
            finally
            {
                _shell.RemoveUser(originalUser.Name);
            }

        }

        [Test]
        public void Cant_GetUser_WhenNotExisting()
        {
            var userName = RandomUserName;

            TestDelegate act = () => _shell.GetUser(userName);

            Assert.Throws<InvalidOperationException>(act);
        }

        private Management.LocalUser.Model.LocalUser BuildLocalUser()
        {
            return new Management.LocalUser.Model.LocalUser
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
