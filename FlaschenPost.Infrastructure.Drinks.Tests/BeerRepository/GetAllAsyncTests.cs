using FlaschenPost.Drinks;
using FlaschenPost.Infrastructure.Drinks.Beer;
using FlaschenPost.Infrastructure.Drinks.Beer.Converters;
using FluentAssertions;

namespace FlaschenPost.Infrastructure.Drinks.Tests.BeerRepository
{
    public class GetAllAsyncTests : DeCultureBaseClass
    {
        private readonly Beer.BeerRepository _beerRepository;
        private readonly ITypeConverter<UnitType> _unitTypeConverter;
        private readonly ITypeConverter<Material> _materialConverter;
        private readonly ITypeConverter<Package> _packageConverter;
        private readonly ITypeConverter<PricePerUnit> _pricePerUnitConverter;

        public GetAllAsyncTests() : base()
        {
            _unitTypeConverter = new UnitTypeConverter();
            _materialConverter = new MaterialConverter();
            _packageConverter = new PackageConverter(_unitTypeConverter, _materialConverter);
            _pricePerUnitConverter = new PricePerUnitConverter(_unitTypeConverter);
            _beerRepository = new Beer.BeerRepository(new HttpClient(), new BeerDataMapper(_unitTypeConverter, _materialConverter, _packageConverter, _pricePerUnitConverter));
        }

        [Fact]
        public async Task Given_Proper_DataSource_ReturnData()
        {
            // Arrange
            var uri = new Uri("https://flapotest.blob.core.windows.net/test/ProductData.json");

            // Act
            var result = await _beerRepository.GetAllAsync(uri);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCountGreaterThan(0);
        }
    }
}

