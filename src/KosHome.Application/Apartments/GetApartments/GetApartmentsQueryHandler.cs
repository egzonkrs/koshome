using MediatR;
using System.Linq;
using FluentResults;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using KosHome.Application.Apartments.Specifications;
using KosHome.Domain.Data.Repositories;
using KosHome.Domain.Entities.Apartments;

namespace KosHome.Application.Apartments.GetApartments;

/// <summary>
/// Handler for getting apartments using Ardalis specifications.
/// </summary>
public sealed class GetApartmentsQueryHandler : IRequestHandler<GetApartmentsQuery, Result<List<Apartment>>>
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
    public async Task<Result<List<Apartment>>> Handle(GetApartmentsQuery request, CancellationToken cancellationToken)
    {
        var specification = new ApartmentsWithPaginationSpecification();
        var allApartments = await _apartmentRepository.ListAsync(specification, cancellationToken);
        return Result.Ok(allApartments.ToList());
    }
}