using System.Threading;
using System.Threading.Tasks;
using KosHome.Domain.Data.Abstractions;
using KosHome.Domain.Entities.Apartments;
using KosHome.Domain.ValueObjects.Apartments;
using KosHome.Domain.ValueObjects.Cities;

namespace KosHome.Domain.Data.Repositories;

/// <summary>
/// 
/// </summary>
public interface IApartmentRepository: IRepository<Apartment>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Apartment> GetByTitleAsync(Title title, CancellationToken cancellationToken = default);
}