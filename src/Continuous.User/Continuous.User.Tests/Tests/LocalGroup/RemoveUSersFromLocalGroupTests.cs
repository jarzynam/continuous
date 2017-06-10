using System;
using System.Collections.Generic;
using Continuous.User.LocalUserGroups;
using Continuous.User.Tests.TestHelpers;
using Continuous.User.Tests.TestHelpers.Installer;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.User.Tests.Tests.LocalGroup
{
    [TestFixture]
    public class RemoveUsersFromLocalGroupTests
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
            _generator = new NameGenerator("testRUFLG");
        }

        [TearDown]
        public void Teardown()
        {
            _groupInstaller.Dispose();
            _userInstaller.Dispose();
        }

        [Test]
        public void RemoveUsers_Should_Remove_When_Memeber_Assigned()
        {
            // arrange
            string groupName = _generator.RandomName;
            var userName = _generator.RandomName;

            _userInstaller.Install(userName, _generator.RandomName);
            _groupInstaller.Install(groupName);

            LocalGroupHelper.AssignUser(groupName, userName);
            
            // act
            _shell.RemoveUsers(groupName, new List<string>{userName});
            
            // assert
            var members = LocalGroupHelper.GetMemebers(groupName);

            members.Should().BeEmpty();
        }

        [Test]
        public void RemoveUsers_Throw_When_User_NotAssigned()
        {
            // assert
            string groupName = _generator.RandomName;
            var userName = _generator.RandomName;

            _groupInstaller.Install(groupName);
            _userInstaller.Install(userName, _generator.RandomName);

            // act
            Action act = () => _shell.RemoveUsers(groupName, new List<string> { userName });

            // assert
            act.ShouldThrow<InvalidOperationException>();

        }

        [Test]
        public void AssignUsers_Throws_When_Group_NotExist()
        {
            // arrange
            string groupName = _generator.RandomName;
            var userName = _generator.RandomName;

            // act
            Action act = () => _shell.RemoveUsers(groupName, new List<string> { userName });

            // assert
            act.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void RemoveUsers_Throws_When_ListIsEmpty()
        {
            // arrange
            string groupName = _generator.RandomName;
            var userName = _generator.RandomName;

            _userInstaller.Install(userName, _generator.RandomName);
            _groupInstaller.Install(groupName);

            LocalGroupHelper.AssignUser(groupName, userName);

            // act
            Action act = () => _shell.AssignUsers(groupName, new List<string>());

            // assert
            act.ShouldThrow<InvalidOperationException>();
        }

    }
}
