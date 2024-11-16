using KosHome.Domain.Entities.Countries;
using KosHome.Domain.ValueObjects.Countries;
using KosHome.Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KosHome.Infrastructure.Configurations;

internal sealed class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasKey(country => country.Id);

        builder.Property(entity => entity.Id)
            .HasConversion(new UlidToStringConverter())
            .HasMaxLength(26)
            .IsRequired();
        
        builder.OwnsOne(country => country.CountryName, countryNameBuilder =>
        {
            countryNameBuilder
                .Property(cn => cn.Value)
                .HasMaxLength(100);
        });

        builder.Property(country => country.Alpha3Code)
            .HasConversion(country => country.Value, value => new CountryAlpha3Code(value))
            .HasMaxLength(3)
            .IsRequired();
        
        builder.HasIndex(x => x.Alpha3Code).IsUnique();
    }
}
