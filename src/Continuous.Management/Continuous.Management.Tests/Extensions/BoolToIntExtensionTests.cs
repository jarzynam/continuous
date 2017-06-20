using Continuous.Management.Common.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.Management.Tests.Extensions
{
    [TestFixture]
    public class BoolToIntExtensionTests
    {
        [Test]
        public void ToInteger_Converts_TrueValue()
        {
            // arrange
            var boolValue = true;

            // act
            var intValue = boolValue.ToInteger();

            // assert
            intValue.Should().Be(1);
        }

        [Test]
        public void ToInteger_Converts_FalseValue()
        {
            // arrange
            var boolValue = false;

            // act
            var intValue = boolValue.ToInteger();

            // assert
            intValue.Should().Be(0);
        }
    }
}
