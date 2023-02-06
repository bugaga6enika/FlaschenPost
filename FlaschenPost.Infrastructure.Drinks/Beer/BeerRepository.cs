using System;
using System.Net.Http.Headers;
using System.Text.Json;
using FlaschenPost.Drinks;
using FlaschenPost.Drinks.Beer;
using DomainBeerData = FlaschenPost.Drinks.Beer.BeerData;

namespace FlaschenPost.Infrastructure.Drinks.Beer
{
    internal class BeerRepository : IBeerRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper<BeerData, FlaschenPost.Drinks.Beer.BeerData> _beerDataMapper;

        public BeerRepository(HttpClient httpClient, IMapper<BeerData, DomainBeerData> beerDataMapper)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _beerDataMapper = beerDataMapper ?? throw new ArgumentNullException(nameof(beerDataMapper));
        }

        public async Task<IEnumerable<DomainBeerData>> GetAllAsync(Uri dataSource, CancellationToken cancellationToken = default)
        {
            _ = dataSource ?? throw new ArgumentNullException(nameof(dataSource));

            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = dataSource,
                };

                var response = await _httpClient.SendAsync(request, cancellationToken);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                if (response is not null)
                {
                    try
                    {
                        var beerData = JsonSerializer.Deserialize<IEnumerable<BeerData>>(content, JsonConfiguration.GetOptions());

                        return beerData?.Select(_beerDataMapper.Map).ToList();
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidContractException($"Cannot apply contract {nameof(BeerData)} to a data source", ex);
                    }
                }
            }
            catch (HttpRequestException requestException) when (requestException.StatusCode == System.Net.HttpStatusCode.Forbidden || requestException.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new ArgumentException("Data source is not available or not exists.");
            }

            return default;
        }
    }
}

