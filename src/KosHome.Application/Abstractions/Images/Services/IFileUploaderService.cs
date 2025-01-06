using FluentResults;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace KosHome.Application.Abstractions.Images.Services;

/// <summary>
/// Interface for a service that handles file uploads.
/// </summary>
public interface IFileUploaderService
{
    /// <summary>
    /// Uploads a file to a specified directory and returns the relative path of the uploaded file.
    /// </summary>
    /// <param name="file">The file to be uploaded.</param>
    /// <returns>The relative path of the uploaded file.</returns>
    Task<Result<string>> UploadFileAsync(IFormFile file);
}
