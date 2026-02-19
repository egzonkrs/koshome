using Ardalis.Specification;
using KosHome.Domain.Entities.Users;
using KosHome.Domain.ValueObjects.Users;

namespace KosHome.Application.Users.Specifications;

/// <summary>
/// Specification for finding a user by email.
/// </summary>
public sealed class UserByEmailSpecification : Specification<User>, ISingleResultSpecification<User>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserByEmailSpecification"/> class.
    /// </summary>
    /// <param name="email">The email to search for.</param>
    public UserByEmailSpecification(Email email)
    {
        Query.Where(user => user.Email == email);
    }
}
