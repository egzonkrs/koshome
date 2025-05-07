using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KosHome.Domain.Data.Abstractions;
using KosHome.Domain.Entities.PropertyTypes;

namespace KosHome.Domain.Data.Repositories;

/// <summary>
/// Interface for the repository handling property type entities.
/// </summary>
public interface IPropertyTypeRepository : IRepository<PropertyType>
{
    /// <summary>
    /// Gets a property type by its name.
    /// </summary>
    /// <param name="name">The name of the property type.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The property type entity if found, null otherwise.</returns>
    Task<PropertyType> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets all property types.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of property types.</returns>
    Task<IReadOnlyList<PropertyType>> GetAllAsync(CancellationToken cancellationToken = default);
} 