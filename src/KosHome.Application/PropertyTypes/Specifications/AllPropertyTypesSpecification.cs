using Ardalis.Specification;
using KosHome.Domain.Entities.PropertyTypes;

namespace KosHome.Application.PropertyTypes.Specifications;

/// <summary>
/// Specification for getting all property types ordered by name.
/// </summary>
public sealed class AllPropertyTypesSpecification : Specification<PropertyType>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AllPropertyTypesSpecification"/> class.
    /// </summary>
    public AllPropertyTypesSpecification()
    {
        Query.OrderBy(propertyType => propertyType.Name);
    }
}
