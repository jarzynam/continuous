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
    public class SetUserFullNameTests
    {
        private IUserShell _shell;
        private NameGenerator _generator;
        private UserInstaller _installer;

        [SetUp]
        public void SetUp()
        {
            _shell = new UserShell();
            _generator = new NameGenerator("testSUFN");
            _installer = new UserInstaller();
        }

        [TearDown]
        public void TearDown()
        {
            _installer.Dispose();
        }

        [Test]
        public void SetUserFullName_ChangesProperty()
        {
            // arrange
            var userName = _generator.RandomName;
            var fullName = "new name";

            _installer.Install(userName, _generator.RandomName);

            // act
            _shell.SetUserFullName(userName, fullName);

            // assert
            UserHelper.GetUser(userName).FullName.Should().Be(fullName);
        }

     
        [Test]
        public void SetUserFullName_ThrowsExcpeiton_WhenUserNotExist()
        {
            // arrange
            var userName = _generator.RandomName;
            var fullName = "new fullName";

            // act
            Action act = () => _shell.SetUserFullName(userName, fullName);

            // assert
            act.Should().Throw<MethodInvocationException>();
        }
    }
}
