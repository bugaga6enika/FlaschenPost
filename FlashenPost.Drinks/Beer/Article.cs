using System;
namespace FlaschenPost.Drinks.Beer
{
    public record Article
    {
        public int Id { get; init; }
        public Package Package { get; init; }
        public Price Price { get; init; }
        public UnitType Unit { get; init; }
        public PricePerUnit PricePerUnit { get; init; }
        public string Image { get; init; }
    }
}

