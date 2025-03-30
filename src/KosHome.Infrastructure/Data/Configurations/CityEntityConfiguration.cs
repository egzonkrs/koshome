using KosHome.Domain.Entities.Cities;
using KosHome.Domain.Entities.Countries;
using KosHome.Domain.ValueObjects.Cities;
using KosHome.Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KosHome.Infrastructure.Data.Configurations;

internal sealed class CityConfiguration : IEntityTypeConfiguration<City>
{
    private const string TableName = "citites";

    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.ToTable(TableName);

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
                .HasColumnName(nameof(City.CityName))
                .HasMaxLength(100);
        });

        builder.Property(city => city.Alpha3Code)
            .HasConversion(city => city.Value, value => new CityAlpha3Code(value))
            .HasColumnName(nameof(City.Alpha3Code))
            .HasMaxLength(3)
            .IsRequired();
        
        builder.HasOne<Country>()
            .WithMany()
            .HasForeignKey(city => city.CountryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
