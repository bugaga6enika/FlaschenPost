namespace FlaschenPost.Infrastructure.Drinks.Beer
{
    internal record Article
    {
        public int Id { get; init; }
        public string ShortDescription { get; init; }
        public decimal Price { get; init; }
        public string Unit { get; init; }
        public string PricePerUnitText { get; init; }
        public string Image { get; init; }
    }
}
