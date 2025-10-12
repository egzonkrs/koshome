using FluentResults;

namespace KosHome.Domain.Common;

public static class PaginationErrors
{
    public static CustomFluentError InvalidPageSize()
        => new CustomFluentError("PAGINATION_INVALID_PAGE_SIZE", "Page size must be between 1 and 100.");

    public static CustomFluentError InvalidPageNumber()
        => new CustomFluentError("PAGINATION_INVALID_PAGE_NUMBER", "Page number must be greater than 0.");

    public static CustomFluentError InvalidCursor(string cursor)
        => new CustomFluentError("PAGINATION_INVALID_CURSOR", $"The cursor '{cursor}' is invalid.");
}

