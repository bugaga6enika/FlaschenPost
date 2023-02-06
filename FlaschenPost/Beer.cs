using System;
using FlaschenPost.Drinks;
using FlaschenPost.Drinks.Beer;

namespace FlaschenPost
{
    public class Beer
    {
        public int Id { get; init; }
        public string BrandName { get; init; }
        public string Name { get; init; }
        public string? DescriptionText { get; init; }
        public int ArticleId { get; init; }
        public Package Package { get; init; }
        public Price Price { get; init; }
        public UnitType Unit { get; init; }
        public PricePerUnit PricePerUnit { get; init; }
        public string Image { get; init; }
    }

    public static class BeerExtensions
    {
        public static IEnumerable<Beer> ToBeerFlatten(this IEnumerable<BeerData> dataSource)
        {
            return dataSource.SelectMany(x => x.Articles).Select(x =>
            {
                var beer = dataSource.Where(b => b.Articles.Any(a => a.Id == x.Id)).Single();
                return new Beer
                {
                    Id = beer.Id,
                    BrandName = beer.BrandName,
                    DescriptionText = beer.DescriptionText,
                    Name = beer.Name,
                    ArticleId = x.Id,
                    Package = x.Package,
                    Price = x.Price,
                    PricePerUnit = x.PricePerUnit,
                    Unit = x.Unit,
                    Image = x.Image
                };
            });
        }

        public static IEnumerable<Beer> ToBeerFlattenOrdered(this IEnumerable<BeerData> dataSource)
        {
            return dataSource.ToBeerFlatten().OrderBy(x => x.PricePerUnit.Price.Amount);
        }
    }
}
