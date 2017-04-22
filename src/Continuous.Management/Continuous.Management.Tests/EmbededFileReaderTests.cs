using System;
using System.IO;
using Continuous.Management.Common;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.Management.Tests
{
    [TestFixture]
    public class EmbededFileReaderTests
    {
        private IEmbededFileReader _reader;

        [SetUp]
        public void SetUp()
        {
            _reader = new EmbededFileReader(GetType());
        }

        [Test]
        public void Can_Read_EmbededFile()
        {
            // arrange
            var fileName = "Continuous.Management.Tests.Resources.EmbededTextFile.txt";

            // act 
            var content = _reader.Read(fileName);

            // assert
            content.Should().Be("Test1");
        }

        [Test]
        public void Can_ThrowException_WhenResource_NotFound()
        {
            // arrange
            var fileName = "fakefile";

            // act 
            Action act = () => _reader.Read(fileName);

            // assert
            act.ShouldThrow<FileNotFoundException>().WithMessage("Can't find resource fakefile in assembly Continuous.Management.Tests" );
        }
    }
}
