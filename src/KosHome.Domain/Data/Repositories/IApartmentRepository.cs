using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using KosHome.Domain.Entities.Apartments;
using KosHome.Domain.ValueObjects.Apartments;

namespace KosHome.Domain.Data.Repositories;

/// <summary>
/// Defines database operations for Apartment.
/// </summary>
public interface IApartmentRepository : IRepositoryBase<Apartment>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Apartment> GetByTitleAsync(Title title, CancellationToken cancellationToken = default);
}