using System;
using Continuous.Management.Common.Extensions;
using NUnit.Framework;

namespace Continuous.Management.Library.Tests.Common.Extensions
{
    [TestFixture]
    public class StringToEnumExtensionTests
    {
        [Test]
        public void Can_Parse_String_To_Enum()
        {
            string input = "OptionNumberOne";
            TestEnum expected = TestEnum.OptionNumberOne;

            var actual = input.ToEnum<TestEnum>();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Can_Parse_String_To_Enum_With_Case_Insensitive()
        {
            string input = "OptionNumberTwo";
            TestEnum expected = TestEnum.Optionnumbertwo;

            var actual = input.ToEnum<TestEnum>();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Can_Parse_String_To_Enum_With_Case_Sensitive()
        {
            string input = "OptionNumberTwo";
            TestEnum expected = TestEnum.Optionnumbertwo;

            TestDelegate act = () => input.ToEnum<TestEnum>(false);

            Assert.Throws<ArgumentException>(act);
        }


        [Test]
        public void Can_Parse_String_To_Enum_With_IgnoringSpaces()
        {
            string input = "Option Number One";
            TestEnum expected = TestEnum.OptionNumberOne;


            var actual = input.ToEnum<TestEnum>();

            Assert.AreEqual(expected, actual);
        }

        private enum TestEnum
        {
            OptionNumberOne,
            Optionnumbertwo
        }
    }


}
