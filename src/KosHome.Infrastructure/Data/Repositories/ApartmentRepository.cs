using System;
using System.Threading;
using System.Threading.Tasks;
using KosHome.Domain.Data.Repositories;
using KosHome.Domain.Entities.Apartments;
using KosHome.Domain.ValueObjects.Apartments;
using KosHome.Infrastructure.Data.Abstractions;

namespace KosHome.Infrastructure.Data.Repositories;

/// <summary>
/// Provides EF Core operations for Apartment.
/// </summary>
public sealed class ApartmentRepository : EfCoreRepository<Apartment>, IApartmentRepository 
{
    /// <summary>
    /// Initializes a new instance of the ApartmentRepository class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public ApartmentRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
    
    /// <summary>
    /// Gets an apartment by its title.
    /// </summary>
    /// <param name="title">The apartment title.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The apartment with the specified title.</returns>
    public Task<Apartment> GetByTitleAsync(Title title, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}