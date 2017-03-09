using System.Collections.Generic;
using Continuous.Management.Common.Extensions;
using NUnit.Framework;

namespace Continuous.Management.Library.Tests.Common.Extensions
{
    [TestFixture]
    public class StringCollectionExtensionsTests
    {
        [Test]
        public void Can_Return_FlatString_FormCollection()
        {
            var collection = new List<string>
            {
                "test1",
                "test2",
                "test3"
            };

            var result = collection.ToFlatString();

            Assert.AreEqual("test1 test2 test3", result);
        }

        [Test]
        public void Can_Return_FlatString_FormCollection_WithCustomSeparator()
        {
            var collection = new List<string>
            {
                "test1",
                "test2",
                "test3"
            };

            var result = collection.ToFlatString("CustomSeparator");

            Assert.AreEqual("test1CustomSeparatortest2CustomSeparatortest3", result);
        }

        [Test]
        public void Can_Return_FlatString_FormEmptyCollection()
        {
            var collection = new List<string>();

            var result = collection.ToFlatString("CustomSeparator");

            Assert.AreEqual("", result);
        }
    }
}
