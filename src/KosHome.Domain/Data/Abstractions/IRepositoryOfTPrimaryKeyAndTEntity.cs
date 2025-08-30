using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KosHome.Domain.Data.Abstractions;

/// <summary>
/// The Repository.
/// </summary>
/// <typeparam name="TPrimaryKey">The Primary Key Data Type.</typeparam>
/// <typeparam name="TEntity">The Entity Data Type.</typeparam>
public interface IRepository<TPrimaryKey, TEntity> where TEntity : class, IEntity<TPrimaryKey>
{
    /// <summary>
    /// Gets a list of all Entities.
    /// </summary>
    /// <param name="specification">The <see cref="ISpecification{TEntity}"/>.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>The list of all Entities.</returns>
    Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> specification = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an Entity by the Primary Key.
    /// </summary>
    /// <param name="id">The Primary Key.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>An Entity by the Primary Key.</returns>
    Task<TEntity> GetByIdAsync(TPrimaryKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether an Entity exists.
    /// </summary>
    /// <param name="specification">The <see cref="ISpecification{TEntity}"/>.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>True if the Entity exists, otherwise False.</returns>
    Task<bool> ExistsAsync(ISpecification<TEntity> specification = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether an Entity exists.
    /// </summary>
    /// <param name="id">The Primary Key.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>True if the Entity exists, otherwise False.</returns>
    Task<bool> ExistsAsync(TPrimaryKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Inserts a new Entity.
    /// </summary>
    /// <param name="entity">The Entity.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>The Primary Key of the newly inserted Entity.</returns>
    Task<TPrimaryKey> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing Entity.
    /// </summary>
    /// <param name="entity">The Entity.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an existing Entity.
    /// </summary>
    /// <param name="entity">The Entity.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
}
