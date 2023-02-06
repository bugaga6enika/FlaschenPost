using System;
using FlaschenPost.Drinks;
using FlaschenPost.Infrastructure.Drinks.Beer.Converters;
using FluentAssertions;

namespace FlaschenPost.Infrastructure.Drinks.Tests.Converters
{
    public class PricePerUnitConverterTests : DeCultureBaseClass
    {
        private readonly ITypeConverter<PricePerUnit> _converter;

        public PricePerUnitConverterTests() : base()
        {
            _converter = new PricePerUnitConverter(new UnitTypeConverter());
        }

        [Fact]
        public void Given_Proper_StringRepresantation_Returns_PricePerUnit()
        {
            // Act
            var pricePerUnit = _converter.Convert("(1,80 €/Liter)");

            // Assert
            pricePerUnit.Price.Should().NotBeNull();
            pricePerUnit.Price.Amount.Should().Be(1.80m);
            pricePerUnit.Price.Currency.Should().Be("€");
            pricePerUnit.UnitType.Should().Be(UnitType.Liter);
        }
    }
}

