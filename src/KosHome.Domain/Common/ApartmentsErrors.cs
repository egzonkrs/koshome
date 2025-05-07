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
        
        /// <summary>
        /// Returns an error indicating that the user is not authenticated.
        /// </summary>
        public static Error UnauthorizedAccess() => new CustomFluentError("UNAUTHORIZED_ACCESS", "User not authenticated.");
        
        /// <summary>
        /// Returns an error indicating that the specified property type was not found.
        /// </summary>
        /// <param name="name">The name of the property type that was not found.</param>
        public static Error PropertyTypeNotFound(string name) => 
            new CustomFluentError("PROPERTY_TYPE_NOT_FOUND", $"Property type '{name}' was not found.");
    }
}
