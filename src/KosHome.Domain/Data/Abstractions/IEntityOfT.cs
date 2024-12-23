namespace KosHome.Domain.Data.Abstractions;

/// <summary>
/// The Entity interface.
/// </summary>
/// <typeparam name="TPrimaryKey">The Primary Key Data Type.</typeparam>
public interface IEntity<TPrimaryKey>
{
    /// <summary>
    /// The Primary Key.
    /// </summary>
    TPrimaryKey Id { get; set; }
}