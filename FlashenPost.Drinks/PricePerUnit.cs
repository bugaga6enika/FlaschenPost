using System;
namespace FlaschenPost.Drinks
{
    public record PricePerUnit
    {
        public Price Price { get; init; }
        public UnitType UnitType { get; init; }
    }
}

