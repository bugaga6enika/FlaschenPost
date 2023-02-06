using System;
namespace FlaschenPost.Drinks.Beer
{
    public class BeerAggregate
    {
        private readonly IBeerRepository _beerRepository;

        public BeerAggregate(IBeerRepository beerRepository)
        {
            _beerRepository = beerRepository ?? throw new ArgumentNullException(nameof(beerRepository));
        }

        public async Task<(BeerData? expensive, BeerData? cheapest)> GetMostExpensiveAndCheapestAsync(Uri dataSource, CancellationToken cancellationToken = default)
        {
            _ = dataSource ?? throw new ArgumentNullException(nameof(dataSource));

            var beerData = await _beerRepository.GetAllAsync(dataSource, cancellationToken);
            if (beerData is null || !beerData.Any())
            {
                return (null, null);
            }

            var mostExpensiveBeers = beerData.MaxBy(x => x.Articles.Where(x => x.PricePerUnit.UnitType == UnitType.Liter).Max(x => x.PricePerUnit.Price.Amount));

            var cheapestBeers = beerData.MinBy(x => x.Articles.Where(x => x.PricePerUnit.UnitType == UnitType.Liter).Min(x => x.PricePerUnit.Price.Amount));

            var mostExpensive = mostExpensiveBeers with { Articles = new[] { mostExpensiveBeers.Articles.OrderByDescending(x => x.PricePerUnit.Price.Amount).First() } };

            var cheapest = cheapestBeers with { Articles = new[] { mostExpensiveBeers.Articles.OrderBy(x => x.PricePerUnit.Price.Amount).First() } };

            return (mostExpensive, cheapest);
        }

        public async Task<BeerData?> GetContainsMostBottlesAsync(Uri dataSource, CancellationToken cancellationToken = default)
        {
            _ = dataSource ?? throw new ArgumentNullException(nameof(dataSource));

            var beerData = await _beerRepository.GetAllAsync(dataSource, cancellationToken);
            if (beerData is null || !beerData.Any())
            {
                return default;
            }

            var mostBottlesBeers = beerData.MaxBy(x => x.Articles.Max(x => x.Package.Quantity));
            var mostBottlesBeer = mostBottlesBeers with { Articles = new[] { mostBottlesBeers.Articles.OrderByDescending(x => x.Package.Quantity).First() } };

            return mostBottlesBeer;
        }

        public async Task<IEnumerable<BeerData>?> GetByExactPriceAsync(Uri dataSource, Price price, CancellationToken cancellationToken = default)
        {
            _ = dataSource ?? throw new ArgumentNullException(nameof(dataSource));

            var beerData = await _beerRepository.GetAllAsync(dataSource, cancellationToken);
            if (beerData is null || !beerData.Any())
            {
                return default;
            }

            return beerData.Where(x => x.Articles.Any(x => x.Price.Amount == price.Amount)).Select(x => new BeerData
            {
                Id = x.Id,
                Name = x.Name,
                BrandName = x.BrandName,
                DescriptionText = x.DescriptionText,
                Articles = x.Articles.Where(a => a.Price.Amount == price.Amount).ToList()
            });
        }
    }
}

