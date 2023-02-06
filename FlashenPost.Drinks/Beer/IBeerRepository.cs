using System;
namespace FlaschenPost.Drinks.Beer
{
    public interface IBeerRepository
    {
        public Task<IEnumerable<BeerData>> GetAllAsync(Uri dataSource, CancellationToken cancellationToken = default);
    }
}

