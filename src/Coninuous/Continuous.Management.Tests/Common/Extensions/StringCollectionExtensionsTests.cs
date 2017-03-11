using System.Collections.Generic;
using Continuous.Management.Common.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.Management.Library.Tests.Common.Extensions
{
    [TestFixture]
    public class StringCollectionExtensionsTests
    {
        [Test]
        public void Can_Return_FlatString_FormCollection()
        {
            // arrange
            var collection = new List<string>
            {
                "test1",
                "test2",
                "test3"
            };

            // act
            var result = collection.ToFlatString();

            // assert
            result.Should().Be("test1 test2 test3");
        }

        [Test]
        public void Can_Return_FlatString_FormCollection_WithCustomSeparator()
        {
            // arrange
            var collection = new List<string>
            {
                "test1",
                "test2",
                "test3"
            };

            // act
            var result = collection.ToFlatString("CustomSeparator");

            // assert
            result.Should().Be("test1CustomSeparatortest2CustomSeparatortest3");
        }

        [Test]
        public void Can_Return_FlatString_FormEmptyCollection()
        {
            // arrange
            var collection = new List<string>();

            // act
            var result = collection.ToFlatString("CustomSeparator");

            // assert
            result.Should().BeEmpty();
        }
    }
}
