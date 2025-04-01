using System;
using System.Collections.Generic;
using System.Linq;
using FluentResults;

namespace KosHome.Api.Extensions;


/// <summary>
/// <see cref="IResultBase"/> extensions.
/// </summary>
public static class ResultBaseExtensions
{
    /// <summary>
    /// Convert <see cref="IResultBase"/> Errors to <see cref="Dictionary{TKey,TValue}"/>.
    /// </summary>
    public static Dictionary<string, IEnumerable<string>> ToDictionary(this List<IError> errors) =>
        errors
            .GroupBy(x => x.Message)
            .ToDictionary(x => x.Key, x => x
                .SelectMany(e => e.Reasons.Select(r => r.Message)));

    /// <summary>
    /// Return string representation of all errors.
    /// </summary>
    public static string ToErrorString(this IResultBase result) =>
        string.Join(",",
            result.Errors.ToDictionary().Select(error => $"{error.Key}:{string.Join("|", error.Value)}"));
}
