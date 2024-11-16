using KosHome.Domain.Entities.Cities;
using KosHome.Domain.ValueObjects.Cities;
using KosHome.Domain.ValueObjects.Countries;
using KosHome.Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KosHome.Infrastructure.Configurations;

internal sealed class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.HasKey(city => city.Id);

        builder.Property(entity => entity.Id)
            .HasConversion(new UlidToStringConverter())
            .HasMaxLength(26)
            .IsRequired();
        
        builder.Property(city => city.CountryId)
            .HasConversion(new UlidToStringConverter())
            .HasMaxLength(26)
            .IsRequired();

        builder.OwnsOne(city => city.CityName, cityNameBuilder =>
        {
            cityNameBuilder.Property(cn => cn.Value)
                .HasMaxLength(100);
        });

        builder.Property(city => city.Alpha3Code)
            .HasConversion(city => city.Value, value => new CityAlpha3Code(value))
            .HasMaxLength(3)
            .IsRequired();
    }
}
