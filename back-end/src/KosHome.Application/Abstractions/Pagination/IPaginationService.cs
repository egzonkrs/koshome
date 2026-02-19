using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using FluentResults;
using KosHome.Domain.Common.Pagination;
using KosHome.Domain.Data.Abstractions;

namespace KosHome.Application.Abstractions.Pagination;

public interface IPaginationService
{
    Task<Result<PaginatedResult<TEntity>>> GetPaginatedAsync<TEntity>(
        IRepositoryBase<TEntity> repository,
        IPaginationSpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
        where TEntity : class, IEntity<Ulid>;
}