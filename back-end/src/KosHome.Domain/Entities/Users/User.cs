using System;
using KosHome.Domain.Data.Abstractions;
using KosHome.Domain.Events.Users;
using KosHome.Domain.ValueObjects.Users;

namespace KosHome.Domain.Entities.Users;

/// <summary>
/// Represents a user entity.
/// </summary>
public sealed class User : DomainEntity, IEntity<Ulid>
{
    /// <summary>
    /// Initializes a new instance with specified parameters.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <param name="firstName">User's first name.</param>
    /// <param name="lastName">User's last name.</param>
    /// <param name="email">User's email address.</param>
    private User(Ulid id, FirstName firstName, LastName lastName, Email email)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    private User()
    {
    }

    /// <summary>
    /// The Id of the user.
    /// </summary>
    public Ulid Id { get; set; }
    
    /// <summary>
    /// Gets the user's first name.
    /// </summary>
    public FirstName FirstName { get; private set; }

    /// <summary>
    /// Gets the user's last name.
    /// </summary>
    public LastName LastName { get; private set; }

    /// <summary>
    /// Gets the user's email address.
    /// </summary>
    public Email Email { get; private set; }

    /// <summary>
    /// Gets or sets the Identity Id of Keycloak.
    /// </summary>
    public string IdentityId { get; private set; } = string.Empty;
    
    /// <summary>
    /// Creates a new user instance.
    /// </summary>
    /// <param name="firstName">User's first name.</param>
    /// <param name="lastName">User's last name.</param>
    /// <param name="email">User's email address.</param>
    /// <returns>A new <see cref="User"/> instance.</returns>
    public static User Create(FirstName firstName, LastName lastName, Email email)
    {
        var user = new User(Ulid.NewUlid(), firstName, lastName, email);

        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
        return user;
    }

    /// <summary>
    /// Sets the identity ID.
    /// </summary>
    /// <param name="identityId">The identity ID to set.</param>
    public void SetIdentityId(string identityId)
    {
        IdentityId = identityId;
    }
}