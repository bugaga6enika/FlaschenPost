using FlaschenPost.Drinks;

namespace FlaschenPost.Infrastructure.Drinks.Beer.Converters
{
    internal class UnitTypeConverter : ITypeConverter<UnitType>
    {
        public UnitType Convert(ReadOnlySpan<char> stringRepresentation)
            => stringRepresentation switch
            {
                "Liter" => UnitType.Liter,
                "L" => UnitType.Liter,
                _ => UnitType.Unknown
            };
    }
}
