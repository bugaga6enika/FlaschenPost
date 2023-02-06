namespace FlaschenPost.Infrastructure.Drinks.Beer
{
    internal record BeerData
    {
        public int Id { get; init; }
        public string BrandName { get; init; }
        public string Name { get; init; }
        public string? DescriptionText { get; init; }
        public IReadOnlyList<Article> Articles { get; init; }
    }
}
