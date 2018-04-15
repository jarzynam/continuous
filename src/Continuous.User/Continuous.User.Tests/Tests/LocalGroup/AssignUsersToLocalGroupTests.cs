using System;
using System.Collections.Generic;
using System.Management.Automation;
using Continuous.User.LocalUserGroups;
using Continuous.User.Tests.TestHelpers;
using Continuous.User.Tests.TestHelpers.Installer;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.User.Tests.Tests.LocalGroup
{
    [TestFixture]
    public class AssignUsersToLocalGroupTests
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
            _generator = new NameGenerator("testAUTLG");
        }

        [TearDown]
        public void Teardown()
        {
            _groupInstaller.Dispose();
            _userInstaller.Dispose();
        }

        [Test]
        public void AssignUsers_Should_Add_When_Memeber_Exist()
        {
            // arrange
            string groupName = _generator.RandomName;
            var userName = _generator.RandomName;

            _userInstaller.Install(userName, _generator.RandomName);
            _groupInstaller.Install(groupName);

            // act
            _shell.AssignUsers(groupName, new List<string>{userName});
            
            // assert
            var members = LocalGroupHelper.GetMemebers(groupName);

            members.Should().Contain(userName);
        }

        [Test]
        public void AssignUsers_Throws_When_Member_NotExist()
        {
            string groupName = _generator.RandomName;
            var userName = _generator.RandomName;

            _groupInstaller.Install(groupName);

            // act
            Action act = () => _shell.AssignUsers(groupName, new List<string>{userName});

            // assert
            act.Should().Throw<MethodInvocationException>();
        }

        [Test]
        public void AssignUsers_Throws_When_Member_HasBeenAssigned()
        {
            // arrange
            string groupName = _generator.RandomName;
            var userName = _generator.RandomName;

            _groupInstaller.Install(groupName);
            _userInstaller.Install(userName, _generator.RandomName);

            LocalGroupHelper.AssignUser(groupName, userName);

            // act
            Action act = () => _shell.AssignUsers(groupName, new List<string> { userName });

            // assert
            act.Should().Throw<MethodInvocationException>();
        }

        [Test]
        public void AssignUsers_Throws_When_User_NotExist()
        {
            // arrange
            string groupName = _generator.RandomName;
            var userName = _generator.RandomName;

            _userInstaller.Install(userName, _generator.RandomName);

            // act
            Action act = () => _shell.AssignUsers(groupName, new List<string>{userName});

            // assert
            act.Should().Throw<ExtendedTypeSystemException>();
        }

        [Test]
        public void AssignUsers_Throws_When_ListIsEmpty()
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
            act.Should().Throw<MethodInvocationException>();
        }

    }
}
