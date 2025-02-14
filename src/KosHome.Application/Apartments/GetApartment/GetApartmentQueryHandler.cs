using MediatR;
using FluentResults;
using System.Threading;
using System.Threading.Tasks;
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

        if (apartment is null)
        {
            return Result.Fail<Apartment>($"Apartment with id: {request.Id} not found.");
        }

        return Result.Ok(apartment);
    }
} 