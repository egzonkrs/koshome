using KosHome.Domain.Entities.ApartmentImages;
using KosHome.Domain.Entities.Apartments;
using KosHome.Domain.Entities.Cities;
using KosHome.Domain.Entities.Users;
using KosHome.Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KosHome.Infrastructure.Data.Configurations;

internal sealed class ApartmentConfiguration : IEntityTypeConfiguration<Apartment>
{
    private const string TableName = "apartments";
    
    public void Configure(EntityTypeBuilder<Apartment> builder)
    {
        builder.ToTable(TableName);
        
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
                .HasColumnName(nameof(Apartment.Title))
                .HasMaxLength(255);
        });

        builder.OwnsOne(apartment => apartment.Description, descriptionBuilder =>
        {
            descriptionBuilder.Property(d => d.Value)
                .HasColumnName(nameof(Apartment.Description))
                .HasMaxLength(2000);
        });

        builder.OwnsOne(apartment => apartment.Price, priceBuilder =>
        {
            priceBuilder.Property(p => p.Value)
                .HasColumnName(nameof(Apartment.Price))
                .HasColumnType("decimal(18,2)");
        });

        builder.OwnsOne(apartment => apartment.ListingType, listingTypeBuilder =>
        {
            listingTypeBuilder.Property(lt => lt.Value)
                .HasColumnName(nameof(Apartment.ListingType))
                .HasMaxLength(10);
        });

        builder.OwnsOne(apartment => apartment.PropertyType, propertyTypeBuilder =>
        {
            propertyTypeBuilder.Property(pt => pt.Value)
                .HasColumnName(nameof(Apartment.PropertyType))
                .HasMaxLength(50);
        });

        builder.OwnsOne(apartment => apartment.Address, addressBuilder =>
        {
            addressBuilder.Property(a => a.Value)
                .HasColumnName(nameof(Apartment.Address))
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
        
        builder.Property(apartment => apartment.Latitude)
            .IsRequired();
        
        builder.Property(apartment => apartment.Longitude)
            .IsRequired();

        builder.Property(apartment => apartment.CreatedAt)
            .IsRequired();

        builder.Property(apartment => apartment.UpdatedAt);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(apartment => apartment.UserId)
            .IsRequired();
        
        builder.HasOne<City>()
            .WithMany()
            .HasForeignKey(apartment => apartment.LocationId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany<ApartmentImage>()
            .WithOne()
            .HasForeignKey(image => image.ApartmentId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}