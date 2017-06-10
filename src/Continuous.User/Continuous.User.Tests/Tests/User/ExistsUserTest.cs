using Continuous.User.Tests.TestHelpers;
using Continuous.User.Tests.TestHelpers.Installer;
using Continuous.User.Users;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.User.Tests.Tests.User
{
    [TestFixture]
    public class ExistsUserTests
    {
        [SetUp]
        public void SetUp()
        {
            _shell = new UserShell();
            _generator = new NameGenerator("testEU");
            _installer = new UserInstaller();
        }

        [TearDown]
        public void TearDown()
        {
            _installer.Dispose();
        }

        private IUserShell _shell;
        private NameGenerator _generator;
        private UserInstaller _installer;


        [Test]
        public void Exists_ReturnsTrue_WhenUserExist()
        {
            // arrange
            var name = _generator.RandomName;

            _installer.Install(name, _generator.RandomName);

            // act
            var result = _shell.Exists(name);

            // assert
            result.Should().BeTrue();
        }

        [Test]
        public void Exists_ReturnsFalse_WhenUserNotExist()
        {
            // arrange
            var name = _generator.RandomName;

            // act
            var result = _shell.Exists(name);

            // assert
            result.Should().BeFalse();
        }

    }
}