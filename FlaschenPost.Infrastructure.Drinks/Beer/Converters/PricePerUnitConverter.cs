using System;
using System.Globalization;
using FlaschenPost.Drinks;

namespace FlaschenPost.Infrastructure.Drinks.Beer.Converters
{
    internal class PricePerUnitConverter : ITypeConverter<PricePerUnit>
    {
        private readonly ITypeConverter<UnitType> _unitTypeConverter;

        public PricePerUnitConverter(ITypeConverter<UnitType> unitTypeConverter)
        {
            _unitTypeConverter = unitTypeConverter ?? throw new ArgumentNullException(nameof(unitTypeConverter));
        }

        public PricePerUnit Convert(ReadOnlySpan<char> stringRepresentation)
        {
            var trimedStringRepresentation = stringRepresentation.Trim().TrimStart('(').TrimEnd(')');
            var priceAmountEndIndex = trimedStringRepresentation.IndexOf(' ');
            var priceAmountString = trimedStringRepresentation.Slice(0, priceAmountEndIndex);

            if (!decimal.TryParse(priceAmountString, System.Globalization.NumberStyles.Any, CultureInfo.CurrentCulture, out var priceAmount))
            {
                throw new ArgumentException("Cannot parse price amount represantation");
            }

            var currencyUnitTypeString = trimedStringRepresentation.Slice(priceAmountEndIndex + 1, trimedStringRepresentation.Length - priceAmountEndIndex - 1);

            var separatorIndex = currencyUnitTypeString.IndexOf('/');

            var currency = currencyUnitTypeString.Slice(0, separatorIndex);
            var unitTypeString = currencyUnitTypeString.Slice(separatorIndex + 1, currencyUnitTypeString.Length - separatorIndex - 1);

            var unitType = _unitTypeConverter.Convert(unitTypeString);

            return new PricePerUnit
            {
                Price = Price.CreateFromDecimal(priceAmount, currency.ToString()),
                UnitType = unitType
            };
        }
    }
}

