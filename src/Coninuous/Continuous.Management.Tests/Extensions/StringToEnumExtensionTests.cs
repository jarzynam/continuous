using System;
using Continuous.Management.Common.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.Management.Library.Tests.Extensions
{
    [TestFixture]
    public class StringToEnumExtensionTests
    {
        [Test]
        public void Can_Parse_String_To_Enum()
        {
            // arrange
            string input = "OptionNumberOne";
            TestEnum expected = TestEnum.OptionNumberOne;

            // act
            var actual = input.ToEnum<TestEnum>();

            // assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Can_Parse_String_To_Enum_With_Case_Insensitive()
        {
            // arrange
            string input = "OptionNumberTwo";
            TestEnum expected = TestEnum.Optionnumbertwo;

            // act
            var actual = input.ToEnum<TestEnum>();

            // assert
            actual.Should().Be(expected);
        }

        [Test]
        public void Can_Parse_String_To_Enum_With_Case_Sensitive()
        {
            // arrange
            string input = "OptionNumberTwo";
            TestEnum expected = TestEnum.Optionnumbertwo;

            // act
            Action act = () => input.ToEnum<TestEnum>(false);

            // assert
            act.ShouldThrow<ArgumentException>();
        }


        [Test]
        public void Can_Parse_String_To_Enum_With_IgnoringSpaces()
        {
            // arrange
            string input = "Option Number One";
            TestEnum expected = TestEnum.OptionNumberOne;

            // act
            var actual = input.ToEnum<TestEnum>();

            // assert
            actual.Should().Be(expected);
        }

        private enum TestEnum
        {
            OptionNumberOne,
            Optionnumbertwo
        }
    }


}
