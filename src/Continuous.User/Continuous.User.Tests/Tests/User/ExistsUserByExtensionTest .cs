using Continuous.User.Tests.TestHelpers;
using Continuous.User.Tests.TestHelpers.Installer;
using Continuous.User.Users.Model;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.User.Tests.Tests.User
{
    [TestFixture]
    public class ExistsUserByExtensionTests
    {
        [SetUp]
        public void SetUp()
        {
         
            _generator = new NameGenerator("testEU");
            _installer = new UserInstaller();
        }

        [TearDown]
        public void TearDown()
        {
            _installer.Dispose();
        }

        
        private NameGenerator _generator;
        private UserInstaller _installer;


        [Test]
        public void Exists_ReturnsTrue_WhenUserExist()
        {
            // arrange
            var name = _generator.RandomName;

            _installer.Install(name, _generator.RandomName);

            // act
            var result = new LocalUserInfo(name).Exists();

            // assert
            result.Should().BeTrue();
        }

        [Test]
        public void Exists_ReturnsFalse_WhenUserNotExist()
        {
            // arrange
            var name = _generator.RandomName;

            _installer.Install(name, _generator.RandomName);

            var user = new LocalUserInfo(name);

            _installer.Remove(name);
            
            // act
            var result = user.Exists();

            // assert
            result.Should().BeFalse();
        }

    }
}