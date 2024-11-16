using KosHome.Domain.Entities.Countries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KosHome.Infrastructure.Configurations;

internal sealed class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasKey(country => country.Id);

        builder.OwnsOne(country => country.CountryName, countryNameBuilder =>
        {
            countryNameBuilder.Property(cn => cn.Value)
                .HasMaxLength(100);
        });

        builder.OwnsOne(country => country.CountryAlpha3Code, alpha3CodeBuilder =>
        {
            alpha3CodeBuilder.Property(ac => ac.Value)
                .HasMaxLength(3);
        });

        builder.HasIndex(x => x.CountryAlpha3Code).IsUnique();
    }
}
