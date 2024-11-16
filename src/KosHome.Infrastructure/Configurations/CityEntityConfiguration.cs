using KosHome.Domain.Entities.Cities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KosHome.Infrastructure.Configurations;

internal sealed class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.HasKey(city => city.Id);

        builder.Property(city => city.CountryId)
            .IsRequired();

        builder.OwnsOne(city => city.CityName, cityNameBuilder =>
        {
            cityNameBuilder.Property(cn => cn.Value)
                .HasMaxLength(100);
        });

        builder.OwnsOne(city => city.CityAlpha3Code, alpha3CodeBuilder =>
        {
            alpha3CodeBuilder.Property(ac => ac.Value)
                .HasMaxLength(3);
        });
    }
}
