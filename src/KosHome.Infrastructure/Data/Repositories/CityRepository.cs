using System;
using System.Threading;
using System.Threading.Tasks;
using KosHome.Domain.Data.Repositories;
using KosHome.Domain.Entities.Cities;
using KosHome.Domain.ValueObjects.Cities;
using KosHome.Infrastructure.Data.Abstractions;

namespace KosHome.Infrastructure.Data.Repositories;

/// <summary>
/// Provides EF Core operations for City.
/// </summary>
public sealed class CityRepository : EfRepositoryBase<Ulid, City>, ICityRepository
{
    public CityRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public Task<City> GetByNameAsync(CityName cityName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}