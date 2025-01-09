using MediatR;
using System.Linq;
using FluentResults;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using KosHome.Domain.Data.Repositories;
using KosHome.Domain.Entities.Apartments;

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
        var allApartments = await _apartmentRepository.GetAllAsync(cancellationToken: cancellationToken);
        return Result.Ok(allApartments.ToList());
    }
}