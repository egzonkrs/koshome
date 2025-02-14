using System;
using FluentResults;
using KosHome.Domain.Entities.Apartments;
using MediatR;

namespace KosHome.Application.Apartments.GetApartment;

public class GetApartmentQuery : IRequest<Result<Apartment>>
{
    public Ulid Id { get; set; }
}