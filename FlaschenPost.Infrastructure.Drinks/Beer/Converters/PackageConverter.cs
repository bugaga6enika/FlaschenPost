using System.Globalization;
using System.Text;
using FlaschenPost.Drinks;

namespace FlaschenPost.Infrastructure.Drinks.Beer.Converters
{
    internal class PackageConverter : ITypeConverter<Package>
    {
        private readonly ITypeConverter<UnitType> _unitTypeConverter;
        private readonly ITypeConverter<Material> _materialConverter;

        public PackageConverter(ITypeConverter<UnitType> unitTypeConverter, ITypeConverter<Material> materialConverter)
        {
            _unitTypeConverter = unitTypeConverter;
            _materialConverter = materialConverter;
        }

        public Package Convert(ReadOnlySpan<char> stringRepresentation)
        {
            var quantity = ParseQuantity(stringRepresentation);

            var (unit, unitType) = ParseUnitAndType(stringRepresentation);

            var material = ParseMaterial(stringRepresentation);

            return new Package
            {
                Quantity = quantity,
                Unit = unit,
                UnitType = unitType,
                Material = material,
            };
        }

        private static ushort ParseQuantity(ReadOnlySpan<char> stringRepresentation)
        {
            var quantityEndIndex = stringRepresentation.IndexOf(' ');
            var quantityString = stringRepresentation.Slice(0, quantityEndIndex);

            if (!UInt16.TryParse(quantityString, out var quantity))
            {
                throw new ArgumentException("Cannot parse quantity represantation");
            }

            return quantity;
        }

        private (double unit, UnitType unitType) ParseUnitAndType(ReadOnlySpan<char> stringRepresentation)
        {
            var unitStartIndex = stringRepresentation.IndexOf('x') + 1;
            var unitEndIndex = stringRepresentation.IndexOf('(');

            var unitWithTypeString = stringRepresentation.Slice(unitStartIndex, unitEndIndex - unitStartIndex).Trim();

            var unitTypeStringBuilder = new StringBuilder();

            for (int i = unitWithTypeString.Length - 1; i >= 0; i--)
            {
                var item = unitWithTypeString[i];
                if (item == ',')
                {
                    continue;
                }

                if (!char.IsDigit(item))
                {
                    unitTypeStringBuilder.Insert(0, item);
                }
                else
                {
                    break;
                }
            }

            var unitTypeString = unitTypeStringBuilder.ToString();
            var unitString = unitWithTypeString.TrimEnd(unitTypeString);

            if (!double.TryParse(unitString, NumberStyles.Any, CultureInfo.CurrentCulture, out var unit))
            {
                throw new ArgumentException("Cannot parse unit represantation");
            }

            var unitType = _unitTypeConverter.Convert(unitTypeString);

            return (unit, unitType);
        }

        private Material ParseMaterial(ReadOnlySpan<char> stringRepresentation)
        {
            var materialStartIndex = stringRepresentation.IndexOf('(') + 1;
            var materialEndIndex = stringRepresentation.IndexOf(')');
            var materialString = stringRepresentation.Slice(materialStartIndex, materialEndIndex - materialStartIndex);

            return _materialConverter.Convert(materialString);
        }
    }
}

