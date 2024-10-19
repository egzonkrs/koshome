using System;

namespace KosHome.Domain.ValueObjects.Countries;

/// <summary>
/// Represents the Alpha-3 code of a country.
/// </summary>
public sealed record Alpha3Code(string Value)
{
    /// <summary>
    /// Creates a new <see cref="Alpha3Code"/> instance after validation.
    /// </summary>
    /// <param name="value">The Alpha-3 country code.</param>
    /// <returns>A valid <see cref="Alpha3Code"/> instance.</returns>
    /// <exception cref="ArgumentException">Thrown when the code is invalid.</exception>
    public static Alpha3Code Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length != 3)
        {
            throw new ArgumentException("Alpha-3 code must be a 3-letter code.", nameof(value));
        }

        return new Alpha3Code(value.ToUpperInvariant());
    }
}
