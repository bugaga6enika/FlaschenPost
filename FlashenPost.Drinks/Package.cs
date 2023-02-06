using System;
namespace FlaschenPost.Drinks
{
    public record Package
    {
        public ushort Quantity { get; init; }
        public double Unit { get; init; }
        public UnitType UnitType { get; init; }
        public Material Material { get; init; }
    }
}
