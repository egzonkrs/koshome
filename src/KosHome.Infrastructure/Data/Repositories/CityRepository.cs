using KosHome.Domain.Data.Repositories;
using KosHome.Domain.Entities.Cities;
using KosHome.Infrastructure.Data.Abstractions;

namespace KosHome.Infrastructure.Data.Repositories;

/// <summary>
/// Provides EF Core operations for City.
/// </summary>
public sealed class CityRepository : EfCoreRepository<City>, ICityRepository
{
    /// <summary>
    /// Initializes a new instance of the CityRepository class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public CityRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}