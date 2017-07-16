using Continuous.User.Tests.TestHelpers;
using Continuous.User.Tests.TestHelpers.Installer;
using Continuous.User.Users;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.User.Tests.Tests.User
{
    [TestFixture]
    public class SetUserVisibilityTests
    {
        private IUserShell _shell;
        private NameGenerator _generator;
        private UserInstaller _installer;

        [SetUp]
        public void SetUp()
        {
            _shell = new UserShell();
            _generator = new NameGenerator("testSUV");
            _installer = new UserInstaller();

            UserHelper.AddSpecialAccountRegistry();
        }

        [TearDown]
        public void TearDown()
        {
            _installer.Dispose();
            UserHelper.RemoveSpecialAccountRegistry();
        }

        [Test]
        public void SetUserVisibility_ToFalse()
        {
            // arrange
            var userName = _generator.RandomName;
            _installer.Install(userName, _generator.RandomName);

            UserHelper.GetUserVisibility(userName).Should().BeTrue();

            // act
            _shell.SetUserVisibility(userName, false);

            // assert
            UserHelper.GetUserVisibility(userName).Should().BeFalse();
        }

        [Test]
        public void SetUserVisibility_ToFalse_WhenRegistryNotExist()
        {
            // arrange
            var userName = _generator.RandomName;
            _installer.Install(userName, _generator.RandomName);

            UserHelper.RemoveSpecialAccountRegistry();
            
            // act
            _shell.SetUserVisibility(userName, false);

            // assert
            UserHelper.GetUserVisibility(userName).Should().BeFalse();
        }

        [Test]
        public void SetUserVisibility_ToFalse_WhenAlreadySetToFalse()
        {
            // arrange
            var userName = _generator.RandomName;
            _installer.Install(userName, _generator.RandomName);

            UserHelper.HideUser(userName);
            UserHelper.GetUserVisibility(userName).Should().BeFalse();

            // act
            _shell.SetUserVisibility(userName, false);

            // assert
            UserHelper.GetUserVisibility(userName).Should().BeFalse();
        }

        [Test]
        public void SetUserVisibility_ToTrue()
        {
            // arrange
            var userName = _generator.RandomName;
            _installer.Install(userName, _generator.RandomName);
            
            UserHelper.HideUser(userName);
            UserHelper.GetUserVisibility(userName).Should().BeFalse();
            // act
            _shell.SetUserVisibility(userName, true);

            // assert
            UserHelper.GetUserVisibility(userName).Should().BeTrue();
        }

        [Test]
        public void SetUserVisibility_ToTrue_WhenIsTrue()
        {
            // arrange
            var userName = _generator.RandomName;
            _installer.Install(userName, _generator.RandomName);
            UserHelper.GetUserVisibility(userName).Should().BeTrue();

            // act
            _shell.SetUserVisibility(userName, true);

            // assert
            UserHelper.GetUserVisibility(userName).Should().BeTrue();
        }

        [Test]
        public void SetUserVisibility_ToTrue_WhenRegistryNotExist()
        {
            // arrange
            var userName = _generator.RandomName;
            _installer.Install(userName, _generator.RandomName);

            UserHelper.RemoveSpecialAccountRegistry();

            // act
            _shell.SetUserVisibility(userName, true);

            // assert
            UserHelper.GetUserVisibility(userName).Should().BeTrue();
        }
    }

}
