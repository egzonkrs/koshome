using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification.EntityFrameworkCore;
using KosHome.Domain.Data.Repositories;
using KosHome.Domain.Entities.Countries;

namespace KosHome.Infrastructure.Data.Repositories;

/// <summary>
/// Country repository implementation using Ardalis.Specification.
/// </summary>
internal sealed class CountryRepository : RepositoryBase<Country>, ICountryRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CountryRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public CountryRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

} 