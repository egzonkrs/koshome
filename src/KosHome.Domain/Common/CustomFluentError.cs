using FluentResults;
namespace KosHome.Domain.Common;

/// <summary>
/// A custom FluentResults error that includes a Code and a Message/Reason.
/// </summary>
public sealed class CustomFluentError : Error
{
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
        Metadata.Add("Code", code);
        Metadata.Add("Reason", message);
    }
}
