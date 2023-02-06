using System;
using FlaschenPost.Drinks;
using FlaschenPost.Infrastructure.Drinks.Beer.Converters;
using FluentAssertions;

namespace FlaschenPost.Infrastructure.Drinks.Tests.Converters
{
    public class UnitTypeConverterTests
    {
        private readonly ITypeConverter<UnitType> _converter;

        public UnitTypeConverterTests()
        {
            _converter = new UnitTypeConverter();
        }

        [Fact]
        public void Given_Liter_Returns_Liter()
        {
            // Act
            var result = _converter.Convert("Liter");

            // Assert
            result.Should().Be(UnitType.Liter);
        }

        [Fact]
        public void Given_L_Returns_Liter()
        {
            // Act
            var result = _converter.Convert("L");

            // Assert
            result.Should().Be(UnitType.Liter);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("liter")]
        [InlineData("Oz")]
        public void Given_Anything_Except_Liter_Returns_Unknown(string unitType)
        {
            // Act
            var result = _converter.Convert(unitType);

            // Assert
            result.Should().Be(UnitType.Unknown);
        }
    }
}

