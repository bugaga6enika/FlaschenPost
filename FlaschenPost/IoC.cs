using System;
using FlaschenPost.Drinks;
using FlaschenPost.Drinks.Beer;
using FlaschenPost.Infrastructure.Drinks.Beer;
using FlaschenPost.Infrastructure.Drinks.Beer.Converters;

namespace FlaschenPost
{
    internal static class IoC
    {
        public static void AddFlaschenPostService(this IServiceCollection services)
        {
            services.AddSingleton<ITypeConverter<UnitType>, UnitTypeConverter>();
            services.AddSingleton<ITypeConverter<Material>, MaterialConverter>();
            services.AddSingleton<ITypeConverter<PricePerUnit>, PricePerUnitConverter>();
            services.AddSingleton<ITypeConverter<Package>, PackageConverter>();
            services.AddSingleton<IMapper<Infrastructure.Drinks.Beer.BeerData, Drinks.Beer.BeerData>, BeerDataMapper>();
            services.AddHttpClient<IBeerRepository, BeerRepository>();
            services.AddTransient<BeerAggregate>();
        }
    }
}

