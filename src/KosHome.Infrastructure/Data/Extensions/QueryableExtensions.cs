using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Threading.Tasks;
using KosHome.Infrastructure.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace KosHome.Infrastructure.Data.Extensions;

/// <summary>
/// Extension methods for IQueryable to support filtering, sorting, and pagination.
/// </summary>
internal static class QueryableExtensions
{
    /// <summary>
    /// Applies sorting to a queryable based on sort criteria.
    /// </summary>
    /// <typeparam name="TSort">The type to sort.</typeparam>
    /// <param name="source">The source queryable.</param>
    /// <param name="sort">The sort criteria.</param>
    /// <returns>The sorted queryable.</returns>
    public static IQueryable<TSort> Order<TSort>(this IQueryable<TSort> source, IList<ISort> sort)
    {
        var query = source;

        var orderBy = sort.FirstOrDefault();
        var thenBy = sort.Skip(1).ToList();

        query = Order(query, orderBy, true);
        thenBy.ForEach(t => query = Order(query, t, false));

        return query;
    }

    /// <summary>
    /// Applies sorting to a queryable based on a single sort criterion.
    /// </summary>
    /// <typeparam name="TSort">The type to sort.</typeparam>
    /// <param name="source">The source queryable.</param>
    /// <param name="sort">The sort criterion.</param>
    /// <param name="firstSort">Indicates if this is the first sort operation.</param>
    /// <returns>The sorted queryable.</returns>
    private static IQueryable<TSort> Order<TSort>(this IQueryable<TSort> source, ISort sort, bool firstSort = true)
    {
        return Order(source, sort.PropertyName, sort.IsAscending, firstSort);
    }

    /// <summary>
    /// Applies sorting to a queryable based on property name and direction.
    /// </summary>
    /// <typeparam name="TSort">The type to sort.</typeparam>
    /// <param name="source">The source queryable.</param>
    /// <param name="orderByProperty">The property name to sort by.</param>
    /// <param name="isAscending">Indicates if sorting is ascending.</param>
    /// <param name="firstSort">Indicates if this is the first sort operation.</param>
    /// <returns>The sorted queryable.</returns>
    private static IQueryable<TSort> Order<TSort>(this IQueryable<TSort> source, string orderByProperty, bool isAscending, bool firstSort = true)
    {
        var command = $"{(firstSort ? "Order" : "Then")}{(isAscending ? "By" : "ByDescending")}";

        var type = typeof(TSort);
        var property = type.GetProperty(orderByProperty);
        var parameter = Expression.Parameter(type, "p");
        
        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);
        var resultExpression = Expression.Call(typeof(Queryable), command, [type, property.PropertyType], 
            source.Expression, Expression.Quote(orderByExpression));
        
        return source.Provider.CreateQuery<TSort>(resultExpression);
    }

    /// <summary>
    /// Applies a where condition if the predicate is not null.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="entity">The source queryable.</param>
    /// <param name="predicate">The predicate to apply, or null.</param>
    /// <returns>The filtered queryable.</returns>
    internal static IQueryable<TEntity> WhereNullable<TEntity>(this IQueryable<TEntity> entity,
        Expression<Func<TEntity, bool>> predicate = null)
    {
        return predicate is not null ? entity.Where(predicate) : entity;
    }
        
    /// <summary>
    /// Counts entities with an optional predicate.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="entity">The source queryable.</param>
    /// <param name="predicate">The predicate to apply, or null.</param>
    /// <returns>The count of entities.</returns>
    internal static long LongCountNullable<TEntity>(this IQueryable<TEntity> entity,
        Expression<Func<TEntity, bool>> predicate = null)
    {
        return predicate is not null ? entity.LongCount(predicate) : entity.LongCount();
    }
        
    /// <summary>
    /// Counts entities asynchronously with an optional predicate.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="entity">The source queryable.</param>
    /// <param name="predicate">The predicate to apply, or null.</param>
    /// <returns>A task containing the count of entities.</returns>
    internal static Task<long> LongCountNullableAsync<TEntity>(this IQueryable<TEntity> entity,
        Expression<Func<TEntity, bool>> predicate = null)
    {
        return predicate is not null ? entity.LongCountAsync(predicate) : entity.LongCountAsync();
    }

    /// <summary>
    /// Counts entities with an optional predicate.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="entity">The source queryable.</param>
    /// <param name="predicate">The predicate to apply, or null.</param>
    /// <returns>The count of entities.</returns>
    internal static int CountNullable<TEntity>(this IQueryable<TEntity> entity,
        Expression<Func<TEntity, bool>> predicate = null)
    {
        return predicate is not null ? entity.Count(predicate) : entity.Count();
    }
        
    /// <summary>
    /// Counts entities asynchronously with an optional predicate.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="entity">The source queryable.</param>
    /// <param name="predicate">The predicate to apply, or null.</param>
    /// <returns>A task containing the count of entities.</returns>
    internal static Task<int> CountNullableAsync<TEntity>(this IQueryable<TEntity> entity,
        Expression<Func<TEntity, bool>> predicate = null)
    {
        return predicate is not null ? entity.CountAsync(predicate) : entity.CountAsync();
    }

    /// <summary>
    /// Applies include operations if the include function is not null.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="entity">The source queryable.</param>
    /// <param name="include">The include function to apply, or null.</param>
    /// <returns>The queryable with includes applied.</returns>
    internal static IQueryable<TEntity> IncludeAll<TEntity>(this IQueryable<TEntity> entity,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include) where TEntity : class
    {
        return include is not null ? include(entity) : entity;
    }
}
