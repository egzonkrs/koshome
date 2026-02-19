using MediatR;
using FluentResults;
using System.Threading;
using System.Threading.Tasks;
using KosHome.Domain.Common;
using KosHome.Domain.Data.Repositories;
using KosHome.Domain.Entities.Apartments;

namespace KosHome.Application.Apartments.GetApartment;

public class GetApartmentQueryHandler : IRequestHandler<GetApartmentQuery, Result<Apartment>>
{
    private readonly IApartmentRepository _apartmentRepository;

    public GetApartmentQueryHandler(IApartmentRepository apartmentRepository)
    {
        _apartmentRepository = apartmentRepository;
    }

    public async Task<Result<Apartment>> Handle(GetApartmentQuery request, CancellationToken cancellationToken)
    {
        var apartment = await _apartmentRepository.GetByIdAsync(request.Id, cancellationToken);

        return apartment is null 
            ? Result.Fail<Apartment>(ApartmentsErrors.NotFound(request.Id.ToString())) 
            : Result.Ok(apartment);
    }
} 