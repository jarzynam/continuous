using System;
using System.Management.Automation;
using Continuous.User.Tests.TestHelpers;
using Continuous.User.Tests.TestHelpers.Installer;
using Continuous.User.Users.Model;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.User.Tests.Tests.User
{
    [TestFixture]
    public class SetUserDescriptionByExtensionTests
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
        public void SetUserDescription_ChangesProperty()
        {
            // arrange
            var userName = _generator.RandomName;
            var description = "new description";

            _installer.Install(userName, _generator.RandomName);

            // act
            new LocalUserInfo(userName)
                .Change()
                .Description(description)
                .Apply();

            // assert
            UserHelper.GetDescription(userName).Should().Be(description);
        }

     
        [Test]
        public void SetUserDescription_ThrowsExcpeiton_WhenUserNotExist()
        {
            // arrange
            var userName = _generator.RandomName;
            var description = "new description";

            _installer.Install(userName, _generator.RandomName);

            var user = new LocalUserInfo(userName);
            
            _installer.Remove(userName);

            // act
            Action act = () => user.Change()
            .Description(description)
            .Apply();

            // assert
            act.ShouldThrow<MethodInvocationException>();
        }
    }
}
