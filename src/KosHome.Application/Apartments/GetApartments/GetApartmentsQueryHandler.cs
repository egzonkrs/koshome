using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using KosHome.Domain.Entities.Apartments;
using MediatR;

namespace KosHome.Application.Apartments.GetApartments;

public class GetApartmentsQueryHandler : IRequestHandler<GetApartmentsQuery, Result<List<Apartment>>>
{
    private readonly IApartmentRepository _apartmentRepository;

    public GetApartmentsQueryHandler(IApartmentRepository apartmentRepository)
    {
        _apartmentRepository = apartmentRepository;
    }

    public async Task<Result<List<Apartment>>> Handle(GetApartmentsQuery request, CancellationToken cancellationToken)
    {
        var apartments = await _apartmentRepository.GetAllAsync(cancellationToken);
        return Result.Ok(apartments);
    }
}