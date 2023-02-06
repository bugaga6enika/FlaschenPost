using FlaschenPost.Drinks;

namespace FlaschenPost.Infrastructure.Drinks.Beer.Converters
{
    internal class MaterialConverter : ITypeConverter<Material>
    {
        public Material Convert(ReadOnlySpan<char> stringRepresentation)
         => stringRepresentation switch
         {
             "Glas" => Material.Glass,
             _ => Material.Unknown
         };
    }
}
