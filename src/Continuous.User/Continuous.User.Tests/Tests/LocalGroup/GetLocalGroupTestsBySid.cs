using Continuous.User.LocalUserGroups;
using Continuous.User.Tests.TestHelpers;
using Continuous.User.Tests.TestHelpers.Installer;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.User.Tests.Tests.LocalGroup
{
    [TestFixture]
    public class GetLocalGroupBySidTests
    {
        private ILocalUserGroupShell _shell;
        private LocalGroupInstaller _groupInstaller;
        private UserInstaller _userInstaller;
        private NameGenerator _generator;

        [SetUp]
        public void SetUp()
        {
            _shell = new LocalUserGroupShell();
            _groupInstaller = new LocalGroupInstaller();
            _userInstaller = new UserInstaller();
            _generator = new NameGenerator("testGLGBS");
        }

        [TearDown]
        public void Teardown()
        {
            _groupInstaller.Dispose();
            _userInstaller.Dispose();
        }


        [Test]
        public void GetBySid_Should_Fetch_LocalGroup_When_HasNot_AssignedMembers()
        {
            // arrange
            string groupName = _generator.RandomName;

            _groupInstaller.Install(groupName);
            var sid = LocalGroupHelper.GetSid(groupName);

            // act
            var group = _shell.GetBySid(sid);

            // assert
            group.Name.Should().Be(groupName);
            group.Members.Should().BeEmpty();
            group.Description.Should().Be(_groupInstaller.Description);
        }

        [Test]
        public void GetBySid_Should_Fetch_LocalGroup_When_Has_AssignedMembers()
        {
            // arrange
            string groupName = _generator.RandomName;
            string userName = _generator.RandomName;
            string userName2 = _generator.RandomName;

            _groupInstaller.Install(groupName);
            _userInstaller.Install(userName, _generator.RandomName);
            _userInstaller.Install(userName2, _generator.RandomName);

            LocalGroupHelper.AssignUser(groupName, userName);
            LocalGroupHelper.AssignUser(groupName, userName2);

            var sid = LocalGroupHelper.GetSid(groupName);
            // act
            var group = _shell.GetBySid(sid);

            // assert
            group.Members.Should().Contain(p => p == userName);
            group.Members.Should().Contain(p => p == userName2);
        }


        [Test]
        public void GetBySid_Return_Null_When_LocalGroup_NotExist()
        {
            // arrange
            var groupName = _generator.RandomName;

            // act
            var group = _shell.GetBySid(groupName);

            // assert
            group.Should().BeNull();
        }
    }
}
