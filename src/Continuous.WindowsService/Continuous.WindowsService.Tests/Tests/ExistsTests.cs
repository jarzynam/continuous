using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Continuous.Management;
using Continuous.WindowsService.Shell;
using Continuous.WindowsService.Tests.TestHelpers;
using Continuous.WindowsService.Tests.TestHelpers.Installer;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Continuous.WindowsService.Tests.Tests
{
    [TestFixture]
    public class ExistsTests
    {
        private NameGenerator _nameGenerator;
        private ServiceInstaller _serviceInstaller;
        private IWindowsServiceShell _shell;

        private const string Prefix = "etTestService";

        [SetUp]
        public void SetUp()
        {
            _shell = new ContinuousManagementFactory().WindowsServiceShell();
            _serviceInstaller = new ServiceInstaller();

            _nameGenerator = new NameGenerator();
        }

        [TearDown]
        public void TearDown()
        {
            _serviceInstaller.Dispose();
        }

        [Test]
        public void Exists_Returns_True_If_ServiceExist()
        {
            // Arrange 
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            _serviceInstaller.InstallService(serviceName);

            // Act
            var result = _shell.Exists(serviceName);

            // Assert
            result.Should().BeTrue();
        }


        [Test]
        public void Exists_Returns_False_If_ServiceNotExist()
        {
            // Arrange 
            var serviceName = _nameGenerator.GetRandomName(Prefix);

            // Act
            var result = _shell.Exists(serviceName);

            // Assert
            result.Should().BeFalse();
        }
    }
}
