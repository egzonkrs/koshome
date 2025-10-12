using Ardalis.Specification.EntityFrameworkCore;
using KosHome.Domain.Data.Repositories;
using KosHome.Domain.Entities.Cities;

namespace KosHome.Infrastructure.Data.Repositories;

/// <summary>
/// City repository implementation using Ardalis.Specification.
/// </summary>
internal sealed class CityRepository : RepositoryBase<City>, ICityRepository
{
    public CityRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}