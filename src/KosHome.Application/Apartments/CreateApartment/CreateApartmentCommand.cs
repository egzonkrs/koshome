using System;
using FluentResults;
using MediatR;

namespace KosHome.Application.Apartments.CreateApartment;

public sealed class CreateApartmentCommand : IRequest<Result<Ulid>>
{
    /// <summary>
    /// See <see cref="KosHome.Domain.Entities.Apartments.Apartment.Title"/>
    /// </summary>
    public string Title { get; init; }

    /// <summary>
    /// See <see cref="KosHome.Domain.Entities.Apartments.Apartment.Description"/>
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// See <see cref="KosHome.Domain.Entities.Apartments.Apartment.Price"/>
    /// </summary>
    public decimal Price { get; init; }

    /// <summary>
    /// See <see cref="KosHome.Domain.Entities.Apartments.Apartment.ListingType"/>
    /// </summary>
    public string ListingType { get; init; }

    /// <summary>
    /// See <see cref="KosHome.Domain.Entities.Apartments.Apartment.PropertyType"/>
    /// </summary>
    public string PropertyType { get; init; }

    /// <summary>
    /// See <see cref="KosHome.Domain.Entities.Apartments.Apartment.Address"/>
    /// </summary>
    public string Address { get; init; }

    /// <summary>
    /// See <see cref="KosHome.Domain.Entities.Apartments.Apartment.LocationId"/>
    /// </summary>
    public Ulid LocationId { get; init; }

    /// <summary>
    /// See <see cref="KosHome.Domain.Entities.Apartments.Apartment.Bedrooms"/>
    /// </summary>
    public int Bedrooms { get; init; }

    /// <summary>
    /// See <see cref="KosHome.Domain.Entities.Apartments.Apartment.Bathrooms"/>
    /// </summary>
    public int Bathrooms { get; init; }

    /// <summary>
    /// See <see cref="KosHome.Domain.Entities.Apartments.Apartment.SquareMeters"/>
    /// </summary>
    public int SquareMeters { get; init; }

    /// <summary>
    /// See <see cref="KosHome.Domain.Entities.Apartments.Apartment.Latitude"/>
    /// </summary>
    public double Latitude { get; init; }

    /// <summary>
    /// See <see cref="KosHome.Domain.Entities.Apartments.Apartment.Longitude"/>
    /// </summary>
    public double Longitude { get; init; }
}