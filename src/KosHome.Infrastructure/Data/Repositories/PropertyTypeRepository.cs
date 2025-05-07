using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KosHome.Domain.Data.Repositories;
using KosHome.Domain.Entities.PropertyTypes;
using KosHome.Infrastructure.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace KosHome.Infrastructure.Data.Repositories;

public class PropertyTypeRepository : EfRepositoryBase<PropertyType>, IPropertyTypeRepository
{
    private readonly DbSet<PropertyType> _dbSet;
    
    public PropertyTypeRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbSet = dbContext.Set<PropertyType>();
    }

    public Task<PropertyType> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return _dbSet
            .Where(pt => pt.Name == name)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PropertyType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .OrderBy(pt => pt.Name)
            .ToListAsync(cancellationToken);
    }
} 