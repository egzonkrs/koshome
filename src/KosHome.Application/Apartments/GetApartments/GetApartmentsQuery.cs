using System.Collections.Generic;
using FluentResults;
using KosHome.Domain.Entities.Apartments;
using MediatR;

namespace KosHome.Application.Apartments.GetApartments;

public class GetApartmentsQuery : IRequest<Result<List<Apartment>>>
{
}