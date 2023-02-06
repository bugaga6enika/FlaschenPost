using System;
using System.Globalization;
using FlaschenPost.Drinks;
using FlaschenPost.Infrastructure.Drinks.Beer.Converters;
using FluentAssertions;

namespace FlaschenPost.Infrastructure.Drinks.Tests.Converters
{
    public class PackageConverterTests : DeCultureBaseClass
    {
        private readonly ITypeConverter<UnitType> _unitTypeConverter;
        private readonly ITypeConverter<Material> _materialConverter;
        private readonly ITypeConverter<Package> _packageConverter;

        public PackageConverterTests() : base()
        {
            _unitTypeConverter = new UnitTypeConverter();
            _materialConverter = new MaterialConverter();
            _packageConverter = new PackageConverter(_unitTypeConverter, _materialConverter);
        }

        [Fact]
        public void Given_Proper_StringRepresantation_Returns_Package()
        {
            // Act
            var package = _packageConverter.Convert("20 x 0,5L (Glas)");

            // Assert
            package.Quantity.Should().Be(20);
            package.Unit.Should().Be(0.5);
            package.UnitType.Should().Be(UnitType.Liter);
            package.Material.Should().Be(Material.Glass);
        }
    }
}

