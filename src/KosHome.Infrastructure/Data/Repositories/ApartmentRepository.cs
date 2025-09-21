using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification.EntityFrameworkCore;
using KosHome.Application.Apartments.Specifications;
using KosHome.Domain.Data.Repositories;
using KosHome.Domain.Entities.Apartments;
using KosHome.Domain.ValueObjects.Apartments;

namespace KosHome.Infrastructure.Data.Repositories;

/// <summary>
/// Apartment repository implementation using Ardalis.Specification.
/// </summary>
internal sealed class ApartmentRepository : RepositoryBase<Apartment>, IApartmentRepository 
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApartmentRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public ApartmentRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
    
    /// <inheritdoc />
    public async Task<Apartment> GetByTitleAsync(Title title, CancellationToken cancellationToken = default)
    {
        var specification = new ApartmentByTitleSpecification(title);
        return await FirstOrDefaultAsync(specification, cancellationToken);
    }

}