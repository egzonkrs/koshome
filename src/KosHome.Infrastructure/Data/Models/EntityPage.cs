namespace KosHome.Infrastructure.Data.Models;

/// <summary>
/// Represents a paged result set for entities.
/// </summary>
/// <typeparam name="TEntity">The type of entity in the page.</typeparam>
public sealed class EntityPage<TEntity> : Paged<TEntity> where TEntity : class
{
}
