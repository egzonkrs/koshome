using MediatR;
using FluentResults;
using System.Threading;
using System.Threading.Tasks;
using KosHome.Application.Apartments.Common;
using KosHome.Application.Apartments.Specifications;
using KosHome.Application.Mappers;
using KosHome.Domain.Common.Pagination;
using KosHome.Domain.Data.Repositories;

namespace KosHome.Application.Apartments.GetApartments;

/// <summary>
/// Handler for getting apartments with enhanced pagination and filtering support.
/// </summary>
public sealed class GetApartmentsQueryHandler : IRequestHandler<GetApartmentsQuery, Result<PaginatedResult<ApartmentResponse>>>
{
    private readonly IApartmentRepository _apartmentRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetApartmentsQueryHandler"/> class.
    /// </summary>
    /// <param name="apartmentRepository">The apartment repository.</param>
    public GetApartmentsQueryHandler(IApartmentRepository apartmentRepository)
    {
        _apartmentRepository = apartmentRepository;
    }

    /// <inheritdoc />
    public async Task<Result<PaginatedResult<ApartmentResponse>>> Handle(GetApartmentsQuery request, CancellationToken cancellationToken)
    {
        var specification = new ApartmentsPaginationSpecification(
            request.PaginationRequest,
            request.CityId,
            request.MinPrice,
            request.MaxPrice,
            request.SearchTerm);
            
        var paginatedApartments = await _apartmentRepository.GetPaginatedAsync(specification, cancellationToken);
        var paginatedResponse = paginatedApartments.Map(apartment => apartment.ToResponse());
        return Result.Ok(paginatedResponse);
    }
}