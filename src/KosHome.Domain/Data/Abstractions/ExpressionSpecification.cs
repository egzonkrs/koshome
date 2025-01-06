using System;
using System.Linq.Expressions;

namespace KosHome.Domain.Data.Abstractions;

/// <summary>
/// A Specification using Lambda Expressions.
/// </summary>
/// <typeparam name="TEntity">The Entity Data Type.</typeparam>
public abstract class ExpressionSpecification<TEntity> : ISpecification<TEntity>
{
    private Func<TEntity, bool> _compiledFunc;
    private Expression<Func<TEntity, bool>> _expression;

    /// <inheritdoc />
    public bool IsSatisfiedBy(TEntity candidate)
    {
        return _compiledFunc is null || _compiledFunc(candidate);
    }

    /// <summary>
    /// Expression that defines the specification.
    /// </summary>
    public Expression<Func<TEntity, bool>> Expression
    {
        set
        {
            _expression = value;
            _compiledFunc = value.Compile();
        }
        get => _expression;
    }

    /// <inheritdoc />
    public uint? PageSize { get; set; }

    /// <inheritdoc />
    public uint PageNumber { get; set; }
}