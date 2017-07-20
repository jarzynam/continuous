using System.Threading;
using Continuous.User.Tests.TestHelpers;
using Continuous.User.Tests.TestHelpers.Installer;
using Continuous.User.Users.Model;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.User.Tests.Tests.User
{
    [TestFixture]
    public class SetPasswordExpiredByExtensionTests
    {
        private NameGenerator _generator;
        private UserInstaller _installer;

        [SetUp]
        public void SetUp()
        {
            _generator = new NameGenerator("testSPE");
            _installer = new UserInstaller();
        }

        [TearDown]
        public void TearDown()
        {
            _installer.Dispose();
        }

        [Test]
        public void SetPasswordExpired_ToTrue_ChangesProperty()
        {
            // arrange
            var userName = _generator.RandomName;

            _installer.Install(userName, _generator.RandomName);

            // act
            new LocalUserInfo(userName).Change()
                .PasswordExpired(true)
                .Apply();

            // assert
            UserHelper.GetPasswordExpired(userName).Should().BeTrue();
        }

        [Test]
        public void SetPasswordExpired_ToTrue_AlsoSetPasswordAge()
        {
            // arrange
            var userName = _generator.RandomName;

            _installer.Install(userName, _generator.RandomName);

            Thread.Sleep(1000); // must wait until password aged

            UserHelper.GetPassowrdAge(userName).Seconds.Should().BePositive();

            // act
            new LocalUserInfo(userName).Change()
                .PasswordExpired(true)
                .Apply();
            
            // assert
            UserHelper.GetPassowrdAge(userName).Seconds.Should().Be(0);
        }

        [Test]
        public void SetPasswordExpired_ToFalse_ChangesProperty()
        {
            // arrange
            var userName = _generator.RandomName;

            _installer.Install(userName, _generator.RandomName);

            UserHelper.SetPasswordExipred(userName, true);
            UserHelper.GetPasswordExpired(userName).Should().BeTrue();

            // act
            new LocalUserInfo(userName).Change()
                .PasswordExpired(false)
                .Apply();

            // assert
            UserHelper.GetPasswordExpired(userName).Should().BeFalse();
        }
    }
}
