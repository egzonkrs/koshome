namespace KosHome.Application.Abstractions.Auth.Constants;

/// <summary>
/// Holds constant values used across the identity service.
/// </summary>
public static class Constants
{
    /// <summary>
    /// Prefix used for all KosHome-related roles.
    /// </summary>
    public const string KosHomePrefix = "koshome-";

    /// <summary>
    /// The default base role for KosHome.
    /// </summary>
    public const string KosHomeBaseRole = KosHomePrefix + "base";

    /// <summary>
    /// A sample user role (player).
    /// </summary>
    public const string PlayerRole = KosHomePrefix + "user";
}