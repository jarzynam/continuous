using System;
using System.Management.Automation;
using Continuous.User.LocalUserGroups;
using Continuous.User.Tests.TestHelpers;
using Continuous.User.Tests.TestHelpers.Installer;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.User.Tests.Tests.LocalGroup
{
    [TestFixture]
    public class CreateLocalGroupTests
    {
        private ILocalUserGroupShell _shell;
        private LocalGroupInstaller _installer;
        private NameGenerator _generator;

        [SetUp]
        public void SetUp()
        {
            _shell = new LocalUserGroupShell();
            _installer = new LocalGroupInstaller();
            _generator = new NameGenerator("testCLG");
        }

        [TearDown]
        public void Teardown()
        {
            _installer.Dispose();
        }

        [Test]
        public void Create_CreatesGroup_Test()
        {
            // arrange
            var name = _generator.RandomName;
            var description = "test group to delete";

            // act
            _shell.Create(name, description);
            _installer.AddAsInstalled(name);
            
            // assert
            var group = LocalGroupHelper.GetGroup(name);

            group.Name.Should().Be(name);
            group.Description.Should().Be(description);
            group.Members.Should().BeEmpty();
        }

        [Test]
        public void Create_Throws_When_CreateSameGroupTwice()
        {
            // arrange
            var name = _generator.RandomName;
            var description = "test group to delete";

            // act
            _shell.Create(name, description);
            _installer.AddAsInstalled(name);

            Action act = () => _shell.Create(name, description);

            // assert
            act.Should().Throw<MethodInvocationException>();
        }
    }
}
