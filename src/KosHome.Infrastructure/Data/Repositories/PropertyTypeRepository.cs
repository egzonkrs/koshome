using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KosHome.Domain.Data.Repositories;
using KosHome.Domain.Entities.PropertyTypes;
using KosHome.Infrastructure.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace KosHome.Infrastructure.Data.Repositories;

/// <summary>
/// Provides EF Core operations for PropertyType.
/// </summary>
public sealed class PropertyTypeRepository : EfCoreRepository<PropertyType>, IPropertyTypeRepository
{
    /// <summary>
    /// Initializes a new instance of the PropertyTypeRepository class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public PropertyTypeRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    /// <summary>
    /// Gets a property type by its name.
    /// </summary>
    /// <param name="name">The property type name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The property type with the specified name.</returns>
    public Task<PropertyType> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return DbSet
            .Where(pt => pt.Name == name)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Gets all property types ordered by name.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of all property types.</returns>
    public async Task<IReadOnlyList<PropertyType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .OrderBy(pt => pt.Name)
            .ToListAsync(cancellationToken);
    }
} 