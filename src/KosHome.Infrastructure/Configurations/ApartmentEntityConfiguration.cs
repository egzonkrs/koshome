using Microsoft.EntityFrameworkCore;
using KosHome.Domain.Entities.Apartments;
using KosHome.Infrastructure.Converters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KosHome.Infrastructure.Configurations;

internal sealed class ApartmentConfiguration : IEntityTypeConfiguration<Apartment>
{
    public void Configure(EntityTypeBuilder<Apartment> builder)
    {
        builder.HasKey(apartment => apartment.Id);

        builder.Property(entity => entity.Id)
            .HasConversion(new UlidToStringConverter())
            .HasMaxLength(26)
            .IsRequired();

        builder.Property(apartment => apartment.UserId)
            .HasConversion(new UlidToStringConverter())
            .HasMaxLength(26)
            .IsRequired();

        builder.OwnsOne(apartment => apartment.Title, titleBuilder =>
        {
            titleBuilder.Property(t => t.Value)
                .HasMaxLength(255);
        });

        builder.OwnsOne(apartment => apartment.Description, descriptionBuilder =>
        {
            descriptionBuilder.Property(d => d.Value)
                .HasMaxLength(2000);
        });

        builder.OwnsOne(apartment => apartment.Price, priceBuilder =>
        {
            priceBuilder.Property(p => p.Value)
                .HasColumnType("decimal(18,2)");
        });

        builder.OwnsOne(apartment => apartment.ListingType, listingTypeBuilder =>
        {
            listingTypeBuilder.Property(lt => lt.Value)
                .HasMaxLength(10);
        });

        builder.OwnsOne(apartment => apartment.PropertyType, propertyTypeBuilder =>
        {
            propertyTypeBuilder.Property(pt => pt.Value)
                .HasMaxLength(50);
        });

        builder.OwnsOne(apartment => apartment.Address, addressBuilder =>
        {
            addressBuilder.Property(a => a.Value)
                .HasMaxLength(255);
        });

        builder.Property(apartment => apartment.LocationId)
            .HasConversion(new UlidToStringConverter())
            .HasMaxLength(26)
            .IsRequired();

        builder.Property(apartment => apartment.Bedrooms)
            .IsRequired();

        builder.Property(apartment => apartment.Bathrooms)
            .IsRequired();

        builder.Property(apartment => apartment.SquareMeters)
            .IsRequired();

        builder.Property(apartment => apartment.CreatedAt)
            .IsRequired();

        builder.Property(apartment => apartment.UpdatedAt);
    }
}