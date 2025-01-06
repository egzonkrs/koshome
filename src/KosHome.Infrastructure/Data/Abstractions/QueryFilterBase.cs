using System;
using System.Collections.Generic;
using System.Linq;
using KosHome.Domain.Data.Abstractions;

namespace KosHome.Infrastructure.Data.Abstractions;

/// <inheritdoc cref="IQueryFilter" />
public abstract class QueryFilterBase : IQueryFilter
{
    private readonly Dictionary<Type, bool> _types = new Dictionary<Type, bool>();

    /// <summary>
    /// Condition to verify that the Query Filter is applicable for Entity.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    protected abstract bool IsValidType<TEntity>();

    /// <summary>
    /// Execute Query Filter conditions.
    /// </summary>
    /// <param name="queryable"></param>
    /// <returns></returns>
    protected abstract IQueryable<TEntity> ExecuteFilter<TEntity>(IQueryable<TEntity> queryable);

    /// <inheritdoc />
    public IQueryable<TEntity> HasQueryFilter<TEntity>(IQueryable<TEntity> queryable)
    {
        var type = typeof(TEntity);

        if (!_types.TryGetValue(type, out var isValid))
        {
            isValid = IsValidType<TEntity>();
            _types.Add(type, isValid);
        }

        return !isValid ? queryable : ExecuteFilter(queryable);
    }
}