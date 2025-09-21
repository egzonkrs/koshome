using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification.EntityFrameworkCore;
using KosHome.Application.PropertyTypes.Specifications;
using KosHome.Domain.Data.Repositories;
using KosHome.Domain.Entities.PropertyTypes;

namespace KosHome.Infrastructure.Data.Repositories;

/// <summary>
/// Property type repository implementation using Ardalis.Specification.
/// </summary>
internal sealed class PropertyTypeRepository : RepositoryBase<PropertyType>, IPropertyTypeRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyTypeRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public PropertyTypeRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    /// <inheritdoc />
    public async Task<PropertyType> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var specification = new PropertyTypeByNameSpecification(name);
        return await FirstOrDefaultAsync(specification, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<PropertyType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var specification = new AllPropertyTypesSpecification();
        var results = await ListAsync(specification, cancellationToken);
        return results.AsReadOnly();
    }

} 