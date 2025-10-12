using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using KosHome.Application.Abstractions.Pagination;
using KosHome.Application.Apartments.Common;
using KosHome.Application.Apartments.Specifications;
using KosHome.Application.Mappers;
using KosHome.Domain.Common.Pagination;
using KosHome.Domain.Data.Repositories;
using MediatR;

namespace KosHome.Application.Apartments.GetApartments;

public sealed class GetApartmentsQueryHandler : IRequestHandler<GetApartmentsQuery, Result<PaginatedResult<ApartmentResponse>>>
{
    private readonly IApartmentRepository _apartmentRepository;
    private readonly IPaginationService _paginationService;

    public GetApartmentsQueryHandler(
        IApartmentRepository apartmentRepository,
        IPaginationService paginationService)
    {
        _apartmentRepository = apartmentRepository;
        _paginationService = paginationService;
    }

    public async Task<Result<PaginatedResult<ApartmentResponse>>> Handle(
        GetApartmentsQuery request, 
        CancellationToken cancellationToken)
    {
        var specification = new ApartmentsPaginationSpecification(
            request.PaginationRequest,
            request.CityId,
            request.MinPrice,
            request.MaxPrice);
            
        var result = await _paginationService.GetPaginatedAsync(
            _apartmentRepository, 
            specification, 
            cancellationToken);
        
        var isFailedResult = result.IsFailed;
        if (isFailedResult)
        {
            return Result.Fail(result.Errors);
        }

        var paginatedResponse = result.Value.Map(apartment => apartment.ToResponse());
        return Result.Ok(paginatedResponse);
    }
}
