using System;
using System.Threading;
using System.Threading.Tasks;
using KosHome.Domain.Data.Repositories;
using KosHome.Domain.Entities.Apartments;
using KosHome.Domain.ValueObjects.Apartments;
using KosHome.Infrastructure.Data.Abstractions;

namespace KosHome.Infrastructure.Data.Repositories;

public class ApartmentRepository : EfRepositoryBase<Apartment>, IApartmentRepository 
{
    public ApartmentRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
    
    public Task<Apartment> GetByTitleAsync(Title title, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}