using FlaschenPost.Drinks;
using FlaschenPost.Infrastructure.Drinks.Beer.Converters;
using FluentAssertions;

namespace FlaschenPost.Infrastructure.Drinks.Tests.Converters
{
    public class MaterialConverterTests
    {
        private readonly ITypeConverter<Material> _converter;

        public MaterialConverterTests()
        {
            _converter = new MaterialConverter();
        }

        [Fact]
        public void Given_Glas_Returns_Glass()
        {
            // Act
            var result = _converter.Convert("Glas");

            // Assert
            result.Should().Be(Material.Glass);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("glas")]
        [InlineData("Wood")]
        public void Given_Anything_Except_Glas_Returns_Unknown(string unitType)
        {
            // Act
            var result = _converter.Convert(unitType);

            // Assert
            result.Should().Be(Material.Unknown);
        }
    }
}

