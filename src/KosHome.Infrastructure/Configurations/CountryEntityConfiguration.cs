using KosHome.Domain.Entities.Countries;
using KosHome.Domain.ValueObjects.Countries;
using KosHome.Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KosHome.Infrastructure.Configurations;

internal sealed class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    private const string TableName = "countries";

    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable(TableName);

        builder.HasKey(country => country.Id);

        builder.Property(entity => entity.Id)
            .HasConversion(new UlidToStringConverter())
            .HasMaxLength(26)
            .IsRequired();
        
        builder.OwnsOne(country => country.CountryName, countryNameBuilder =>
        {
            countryNameBuilder
                .Property(cn => cn.Value)
                .HasColumnName(nameof(Country.CountryName))
                .HasMaxLength(100);
        });

        builder.Property(country => country.Alpha3Code)
            .HasConversion(country => country.Value, value => new CountryAlpha3Code(value))
            .HasColumnName(nameof(Country.Alpha3Code))
            .HasMaxLength(3)
            .IsRequired();
        
        builder.HasIndex(x => x.Alpha3Code).IsUnique();
    }
}
