using System;
using Continuous.WindowsService.Tests.TestHelpers;
using Continuous.WindowsService.Tests.TestHelpers.Installer;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.WindowsService.Tests.Tests.WindowsServiceInfo
{
    [TestFixture]
    public class UninstallByServiceExtensionTests
    {
        private ServiceInstaller _installer;
        private NameGenerator _generator;


        [SetUp]
        public void SetUp()
        {
            _installer = new ServiceInstaller();
            _generator = new NameGenerator("testUn");
        }

        [TearDown]
        public void TearDown()
        {
            _installer.Dispose();
        }

        [Test]
        public void Uninstall_Should_RemoveService()
        {
            // arrange
            var service = _installer.InstallAndGetService(_generator.RandomName);

            // act
            service.Uninstall();

            // assert
            ServiceHelper.Exist(service.Name).Should().BeFalse();
        }

        [Test]
        public void Uninstall_Should_Throw_WhenNotExist()
        {
            // arrange
            var service = _installer.InstallAndGetService(_generator.RandomName);

            _installer.UninstallService(service.Name);
            
            // act
            Action act = () => service.Uninstall();

            // assert
            act.ShouldThrow<InvalidOperationException>();
        }
    }
}
