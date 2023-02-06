using System;
using FlaschenPost.Drinks;
using DomainBeerData = FlaschenPost.Drinks.Beer.BeerData;

namespace FlaschenPost.Infrastructure.Drinks.Beer
{
    public class BeerDataMapper : IMapper<BeerData, FlaschenPost.Drinks.Beer.BeerData>
    {
        private readonly ITypeConverter<UnitType> _unitTypeConverter;
        private readonly ITypeConverter<Material> _materialConverter;
        private readonly ITypeConverter<Package> _packageConverter;
        private readonly ITypeConverter<PricePerUnit> _pricePerUnitConverter;

        public BeerDataMapper(ITypeConverter<UnitType> unitTypeConverter, ITypeConverter<Material> materialConverter, ITypeConverter<Package> packageConverter, ITypeConverter<PricePerUnit> pricePerUnitConverter)
        {
            _unitTypeConverter = unitTypeConverter ?? throw new ArgumentNullException(nameof(unitTypeConverter));
            _materialConverter = materialConverter ?? throw new ArgumentNullException(nameof(materialConverter));
            _packageConverter = packageConverter ?? throw new ArgumentNullException(nameof(packageConverter));
            _pricePerUnitConverter = pricePerUnitConverter ?? throw new ArgumentNullException(nameof(pricePerUnitConverter));
        }

        DomainBeerData IMapper<BeerData, FlaschenPost.Drinks.Beer.BeerData>.Map(BeerData source)
        {
            return new DomainBeerData
            {
                Id = source.Id,
                Name = source.Name,
                BrandName = source.BrandName,
                DescriptionText = source.DescriptionText,
                Articles = source.Articles.Select(a => new FlaschenPost.Drinks.Beer.Article
                {
                    Id = a.Id,
                    Package = _packageConverter.Convert(a.ShortDescription),
                    Price = Price.CreateFromDecimal(a.Price),
                    Unit = _unitTypeConverter.Convert(a.Unit),
                    PricePerUnit = _pricePerUnitConverter.Convert(a.PricePerUnitText),
                    Image = a.Image

                }).ToList()
            };
        }
    }
}

