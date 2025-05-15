using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KosHome.Domain.Data.Abstractions;
using KosHome.Domain.Entities.Countries;
using KosHome.Domain.ValueObjects.Countries;

namespace KosHome.Domain.Data.Repositories;

/// <summary>
/// Defines database operations for Country.
/// </summary>
public interface ICountryRepository : IRepository<Country>
{
} 