using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace KosHome.Api.Models.Apartments.Requests;

public record CreateApartmentRequest(
    string Title,
    string Description,
    decimal Price,
    string ListingType,
    string PropertyType,
    string Address,
    Ulid CityId,
    int Bedrooms,
    int Bathrooms,
    int SquareMeters,
    double Latitude,
    double Longitude,
    IEnumerable<IFormFile>? Images);
