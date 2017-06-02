using Continuous.User.LocalUserGroups;
using Continuous.User.Tests.TestHelpers;
using Continuous.User.Tests.TestHelpers.Installer;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.User.Tests.Tests.LocalGroup
{
    
    class GetLocalGroupTests
    {
        private ILocalUserGroupShell _shell;
        private LocalGroupInstaller _groupInstaller;
        private NameGenerator _generator;
        private UserInstaller _userInstaller;

        [SetUp]
        public void SetUp()
        {
            _shell = new LocalUserGroupShell();
            _groupInstaller = new LocalGroupInstaller();
            _userInstaller = new UserInstaller();
            _generator = new NameGenerator("testGLG");
        }

        [TearDown]
        public void Teardown()
        {
            _groupInstaller.Dispose();
            _userInstaller.Dispose();
        }

        [Test]
        public void Get_Should_Fetch_LocalGroup_When_HasNot_AssignedMembers()
        {
            // arrange
            string groupName = _generator.RandomName;

            _groupInstaller.Install(groupName);

            // act
            var group = _shell.Get(groupName);

            // assert
            group.Name.Should().Be(groupName);
            group.Members.Should().BeEmpty();
            group.Description.Should().Be(_groupInstaller.Description);
        }

        [Test]
        public void Get_Should_Fetch_LocalGroup_When_Has_AssignedMembers()
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

            // act
            var group = _shell.Get(groupName);

            // assert
            group.Members.Should().Contain(p => p == userName);
            group.Members.Should().Contain(p => p == userName2);
        }


        [Test]
        public void Get_Return_Null_When_LocalGroup_NotExist()
        {
            // arrange
            var groupName = _generator.RandomName;

            // act
            var group =  _shell.Get(groupName);

            // assert
            group.Should().BeNull();
        }
    }
}
