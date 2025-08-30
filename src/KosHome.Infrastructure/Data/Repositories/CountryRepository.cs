using KosHome.Domain.Data.Repositories;
using KosHome.Domain.Entities.Countries;
using KosHome.Infrastructure.Data.Abstractions;

namespace KosHome.Infrastructure.Data.Repositories;

/// <summary>
/// Provides EF Core operations for Country.
/// </summary>
public sealed class CountryRepository : EfCoreRepository<Country>, ICountryRepository
{
    /// <summary>
    /// Initializes a new instance of the CountryRepository class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public CountryRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
} 