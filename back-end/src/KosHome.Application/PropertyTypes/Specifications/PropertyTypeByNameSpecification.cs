using Ardalis.Specification;
using KosHome.Domain.Entities.PropertyTypes;

namespace KosHome.Application.PropertyTypes.Specifications;

/// <summary>
/// Specification for finding a property type by name.
/// </summary>
public sealed class PropertyTypeByNameSpecification : Specification<PropertyType>, ISingleResultSpecification<PropertyType>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyTypeByNameSpecification"/> class.
    /// </summary>
    /// <param name="name">The property type name to search for.</param>
    public PropertyTypeByNameSpecification(string name)
    {
        Query.Where(propertyType => propertyType.Name == name);
    }
}
