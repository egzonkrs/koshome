using FluentResults;

namespace KosHome.Domain.Common
{
    public static class ApartmentsErrors
    {
        /// <summary>
        /// Returns an error indicating that an unexpected error occurred during a apartment related process.
        /// </summary>
        public static Error UnexpectedError()
            => new CustomFluentError("APARTMENTS_UNEXPECTED_ERROR","An unexpected error occurred during apartment creation.");
    
        /// <summary>
        /// Returns an error indicating that a user with the specified ID was not found.
        /// </summary>
        /// <param name="id">The ID of the apartment that was not found.</param>
        public static Error NotFound(string id) => new CustomFluentError("APARTMENT_NOT_FOUND", $"Apartment with Id: `{id}` was not found");
    }
}
