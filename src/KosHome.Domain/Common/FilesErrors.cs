using FluentResults;

namespace KosHome.Domain.Common;

/// <summary>
/// Provides error definitions for file upload operations.
/// </summary>
public static class FilesErrors
{
    public static Error NoFileProvided
        => new CustomFluentError("NO_FILE_PROVIDED", "No file was provided.");

    public static Error MaxFileSizeExceeded(int maxFileSizeMb)
        => new CustomFluentError("MAX_FILE_SIZE_EXCEEDED",
            $"The file size exceeds the maximum allowed size of {maxFileSizeMb} MB.");

    public static Error UnsupportedFileFormat(string allowedTypes)
        => new CustomFluentError("UNSUPPORTED_FILE_FORMAT",
            $"The file extension is not allowed. Allowed types are: {allowedTypes}.");

    public static Error UnexpectedError
        => new CustomFluentError("UPLOAD_UNEXPECTED_ERROR",
            "An unexpected error occurred while uploading the file.");
}