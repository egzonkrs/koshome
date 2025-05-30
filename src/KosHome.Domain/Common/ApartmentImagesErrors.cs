using FluentResults;

namespace KosHome.Domain.Common;

/// <summary>
/// Provides error definitions for apartment image-related operations.
/// </summary>
public static class ApartmentImagesErrors
{
    /// <summary>
    /// Returns an error indicating that an unexpected error occurred during an apartment image operation.
    /// </summary>
    public static CustomFluentError UnexpectedError()
        => new CustomFluentError("APARTMENT_IMAGE_UNEXPECTED_ERROR", "An unexpected error occurred during the apartment image operation.");

    /// <summary>
    /// Returns an error indicating that an apartment image with the specified ID was not found.
    /// </summary>
    /// <param name="id">The ID of the apartment image that was not found.</param>
    public static CustomFluentError NotFound(string id)
        => new CustomFluentError("APARTMENT_IMAGE_NOT_FOUND", $"Apartment image with Id: `{id}` was not found.");

    /// <summary>
    /// Returns an error indicating that no changes were detected during the operation.
    /// </summary>
    public static CustomFluentError NoChangesDetected()
        => new CustomFluentError("NO_CHANGES_DETECTED", "No changes were detected.");

    /// <summary>
    /// Returns an error indicating that the image format is not supported.
    /// </summary>
    /// <param name="supportedFormats">String representation of supported formats.</param>
    public static CustomFluentError UnsupportedImageFormat(string supportedFormats)
        => new CustomFluentError("UNSUPPORTED_IMAGE_FORMAT", $"Image format is not supported. Supported formats: {supportedFormats}");

    /// <summary>
    /// Returns an error indicating that the image size exceeds the maximum allowed.
    /// </summary>
    /// <param name="maxSizeMb">Maximum size in MB.</param>
    public static CustomFluentError ImageSizeTooLarge(int maxSizeMb)
        => new CustomFluentError("IMAGE_SIZE_TOO_LARGE", $"Image size exceeds the maximum allowed size of {maxSizeMb}MB.");

    /// <summary>
    /// Returns an error indicating that no image was provided.
    /// </summary>
    public static CustomFluentError NoImageProvided()
        => new CustomFluentError("NO_IMAGE_PROVIDED", "No image was provided.");

    /// <summary>
    /// Returns an error indicating that the apartment associated with the image was not found.
    /// </summary>
    /// <param name="apartmentId">The ID of the apartment.</param>
    public static CustomFluentError ApartmentNotFound(string apartmentId)
        => new CustomFluentError("APARTMENT_NOT_FOUND_FOR_IMAGE", $"No apartment found with Id: `{apartmentId}` for this image.");

    /// <summary>
    /// Returns an error indicating that an error occurred during image processing.
    /// </summary>
    public static CustomFluentError ImageProcessingError()
        => new CustomFluentError("IMAGE_PROCESSING_ERROR", "An error occurred while processing the image.");

    /// <summary>
    /// Returns an error indicating that image saving failed.
    /// </summary>
    public static CustomFluentError ImageSavingFailed()
        => new CustomFluentError("IMAGE_SAVING_FAILED", "Failed to save the image to the file system.");
} 