using System;
using System.Threading;
using System.Threading.Tasks;
using KosHome.Domain.Data.Abstractions;
using KosHome.Domain.Entities.Cities;
using KosHome.Domain.ValueObjects.Cities;

namespace KosHome.Domain.Data.Repositories;

/// <summary>
/// Defines database operations for City.
/// </summary>
public interface ICityRepository : IRepository<Ulid, City>
{
    Task<City> GetByNameAsync(CityName cityName, CancellationToken cancellationToken = default);
}