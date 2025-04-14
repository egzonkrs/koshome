namespace KosHome.Domain.Abstractions;

/// <summary>
/// The User Context Accessor.
/// </summary>
public interface IUserContextAccessor
{
    /// <summary>
    /// The User's Id.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// The User's Full Name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The User's Email.
    /// </summary>
    string Email { get; }
} 