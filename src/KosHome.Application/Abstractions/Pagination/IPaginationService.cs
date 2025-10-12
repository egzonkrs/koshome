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
    Task<Result<PaginatedResult<T>>> GetPaginatedAsync<T>(
        IRepositoryBase<T> repository,
        IPaginationSpecification<T> specification,
        CancellationToken cancellationToken = default)
        where T : class, IEntity<Ulid>;
}

