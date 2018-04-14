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
    public class SetUserFullNameByExtensionTests
    {
        private NameGenerator _generator;
        private UserInstaller _installer;

        [SetUp]
        public void SetUp()
        {
           
            _generator = new NameGenerator("testSUFNBE");
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
            var fullName = "new fullName";

            _installer.Install(userName, _generator.RandomName);

            // act
            new LocalUserInfo(userName)
                .Change()
                .FullName(fullName)
                .Apply();

            // assert
            UserHelper.GetUser(userName).FullName.Should().Be(fullName);
        }

     
        [Test]
        public void SetUserFullName_ThrowsExcpction_WhenUserNotExist()
        {
            // arrange
            var userName = _generator.RandomName;
            var fullName = "new fullName";

            _installer.Install(userName, _generator.RandomName);

            var user = new LocalUserInfo(userName);
            
            _installer.Remove(userName);

            // act
            Action act = () => user.Change()
            .FullName(fullName)
            .Apply();

            // assert
            act.ShouldThrow<MethodInvocationException>();
        }
    }
}
