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
    public class SetUserDescriptionTests
    {
        private IUserShell _shell;
        private NameGenerator _generator;
        private UserInstaller _installer;

        [SetUp]
        public void SetUp()
        {
            _shell = new UserShell();
            _generator = new NameGenerator("testSPE");
            _installer = new UserInstaller();
        }

        [TearDown]
        public void TearDown()
        {
            _installer.Dispose();
        }

        [Test]
        public void SetUserDescription_ChangesProperty()
        {
            // arrange
            var userName = _generator.RandomName;
            var description = "new description";

            _installer.Install(userName, _generator.RandomName);

            // act
            _shell.SetUserDescription(userName, description);

            // assert
            UserHelper.GetDescription(userName).Should().Be(description);
        }

     
        [Test]
        public void SetUserDescription_ThrowsExcpeiton_WhenUserNotExist()
        {
            // arrange
            var userName = _generator.RandomName;
            var description = "new description";


            // act
            Action act = () => _shell.SetUserDescription(userName, description);

            // assert
            act.Should().Throw<MethodInvocationException>();
        }
    }
}
