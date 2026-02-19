namespace KosHome.Domain.Data.Abstractions;

/// <summary>
/// The Specification.
/// </summary>
/// <typeparam name="TEntity">The Entity Data Type.</typeparam>
/// Keyword 'in' Enables you to use a more derived type than originally specified.
/// Check - https://learn.microsoft.com/en-us/dotnet/standard/generics/covariance-and-contravariance
public interface ISpecification<in TEntity>
{
    /// <summary>
    /// The condition for the Specification.
    /// </summary>
    /// <param name="candidate">The Entity.</param>
    /// <returns></returns>
    bool IsSatisfiedBy(TEntity candidate);

    /// <summary>
    /// The Page Size.
    /// </summary>
    uint? PageSize { get; }

    /// <summary>
    /// The Page Number.
    /// </summary>
    uint PageNumber { get; }
}