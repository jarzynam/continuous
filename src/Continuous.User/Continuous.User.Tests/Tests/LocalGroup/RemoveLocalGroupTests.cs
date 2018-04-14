using System;
using Continuous.User.LocalUserGroups;
using Continuous.User.Tests.TestHelpers;
using Continuous.User.Tests.TestHelpers.Installer;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.User.Tests.Tests.LocalGroup
{
    [TestFixture]
    public class RemoveLocalGroupTests
    {
        private ILocalUserGroupShell _shell;
        private LocalGroupInstaller _installer;
        private NameGenerator _generator;

        [SetUp]
        public void SetUp()
        {
            _shell = new LocalUserGroupShell();
            _installer = new LocalGroupInstaller();
            _generator = new NameGenerator("testRLG");
        }

        [TearDown]
        public void Teardown()
        {
            _installer.Dispose();
        }

        [Test]
        public void Remove_DeletesGroup_Test()
        {
            // arrange
            var name = _generator.RandomName;
            
            _installer.Install(name);
            
            // act
            _shell.Remove(name);

            // assert
            var group = LocalGroupHelper.GetGroup(name);
            group.Should().BeNull();
        }

        [Test]
        public void Remove_Throws_When_Group_NotExist()
        {
            // arrange
            var name = _generator.RandomName;
           
            // act
            Action act = () => _shell.Remove(name);

            // assert
            act.ShouldThrow<System.Management.Automation.MethodInvocationException>();
        }
    }
}
