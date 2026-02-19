using FluentResults;
namespace KosHome.Domain.Common;

/// <summary>
/// A custom FluentResults error that includes a Code and a Message/Reason.
/// </summary>
public sealed class CustomFluentError : Error
{
    /// <summary>
    /// Metadata code key to retrieve the error code.
    /// </summary>
    public const string MetadataCodeKey = "Code";
    
    /// <summary>
    /// Metadata reason key to retrieve the error reason.
    /// </summary>
    public const string MetadataReasonKey = "Reason";

    /// <summary>
    /// A short code that classifies the error (e.g., "Users.NotFound").
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Constructs a custom FluentResults error with the specified code and message (reason).
    /// </summary>
    /// <param name="code">A short classification code.</param>
    /// <param name="message">A descriptive message or reason for the error.</param>
    public CustomFluentError(string code, string message) : base(message)
    {
        Code = code;
        
        // Reasons.Add(new Error(message));
        // Optionally add code/message to metadata for future retrieval
        Metadata.Add(MetadataCodeKey, code);
        Metadata.Add(MetadataReasonKey, message);
    }
    
    public static string? GetErrorCode(IError? error)
    {
        if (error is null) return null;
        
        if (error.Metadata.TryGetValue(MetadataCodeKey, out var metadataCode) && metadataCode is string codeStr)
        {
            return codeStr;
        }

        if (error is CustomFluentError cfe)
        {
            return cfe.Code; // Fallback to the direct property
        }

        return null;
    }

    /// <summary>
    /// Gets the error reason (user-friendly message).
    /// It first attempts to retrieve from metadata using <see cref="CustomFluentError.MetadataReasonKey"/>.
    /// If not found or empty, it returns the error's Message property.
    /// </summary>
    /// <param name="error">The error object.</param>
    /// <returns>The error reason string. Returns string.Empty if the error or its message is null.</returns>
    public static string GetErrorReason(IError error)
    {
        if (error is null)
        {
            return string.Empty;
        }

        if (error.Metadata.TryGetValue(MetadataReasonKey, out var metadataReason) && metadataReason is string reasonStr && !string.IsNullOrEmpty(reasonStr))
        {
            return reasonStr;
        }
        
        return error.Message ?? string.Empty;
    }
}
