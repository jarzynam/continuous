using Continuous.User;
using Continuous.WindowsService;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.Management.Tests
{
    [Ignore("remove this tests due to dlls conflicts")]
    [TestFixture]
    public class ContinuousManagementFactoryTests
    {
        private ContinuousManagementFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new ContinuousManagementFactory();
        }

        [Test]
        public void Can_Create_WindowsServiceShell()
        {
            // Act
            var shell = _factory.WindowsServiceShell();

            // Assert
            shell.Should().NotBeNull();
        }

        [Test]
        public void Can_Create_UserShell()
        {
            // Act
            var shell = _factory.UserShell();

            // Assert
            shell.Should().NotBeNull();
        }

        [Test]
        public void Can_Create_LocalUserGroupShell()
        {
            // Act
            var shell = _factory.LocalUserGroup();

            // Assert
            shell.Should().NotBeNull();
        }
    }
}
